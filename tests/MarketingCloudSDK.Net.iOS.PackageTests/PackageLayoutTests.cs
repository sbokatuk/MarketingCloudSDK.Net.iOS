using System.IO.Compression;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace MarketingCloudSDK.Net.iOS.PackageTests;

/// <summary>What one package is expected to be.</summary>
/// <param name="Id">The NuGet package id.</param>
/// <param name="Framework">The native framework the package binds.</param>
/// <param name="DependsOn">The ids of the sibling packages it must depend on.</param>
public sealed record PackageSpec(string Id, string Framework, string[] DependsOn);

/// <summary>
/// Asserts the shape of the produced NuGet packages. These run against the packed .nupkg rather
/// than the build output, so they catch packaging regressions the compiler cannot see.
/// </summary>
public class PackageLayoutTests
{
    public static readonly string[] ExpectedTargetFrameworks =
    [
        "net8.0-ios18.0", "net9.0-ios18.0", "net10.0-ios26.0",
    ];

    public static readonly PackageSpec[] All = LoadManifest();

    public static TheoryData<string> Ids
    {
        get
        {
            var data = new TheoryData<string>();
            foreach (var package in All)
            {
                data.Add(package.Id);
            }

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Package_carries_a_binding_assembly_for_every_target_framework(string id)
    {
        using var package = OpenPackage(id);

        foreach (var tfm in ExpectedTargetFrameworks)
        {
            Assert.True(
                package.GetEntry($"lib/{tfm}/{id}.dll") is not null,
                $"{id} is missing 'lib/{tfm}/{id}.dll'.");
        }
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Package_ships_the_compressed_native_payload_for_every_target_framework(string id)
    {
        using var package = OpenPackage(id);

        foreach (var tfm in ExpectedTargetFrameworks)
        {
            // CompressBindingResourcePackage=true ships the xcframework as one .resources.zip.
            // The uncompressed directory form trips NU5123 and Windows MAX_PATH; no payload at
            // all means the binding is an empty shell that links nothing.
            var entry = package.GetEntry($"lib/{tfm}/{id}.resources.zip");
            Assert.True(
                entry is not null,
                $"{id} ships no {id}.resources.zip for {tfm} - the native payload is missing.");

            // MarketingCloudSDK is ~10 MB per slice; AppGroupSDK ~1 MB.
            var floor = PayloadOnly.Contains(id) ? 200_000 : 1_000_000;
            Assert.True(
                entry!.Length > floor,
                $"'{entry.FullName}' is only {entry.Length} bytes, which is too small to hold the xcframework.");
        }
    }

    /// <summary>
    /// Packages that deliberately ship their framework without projecting it. Stated here rather
    /// than derived, so a surface appearing in one - or vanishing from a real binding - fails.
    /// </summary>
    public static readonly string[] PayloadOnly = ["MarketingCloudSDK.Net.AppGroupSDK.iOS"];

    [Theory]
    [MemberData(nameof(Ids))]
    public void Binding_assembly_projects_exactly_what_it_should(string id)
    {
        var spec = All.Single(package => package.Id == id);
        using var package = OpenPackage(id);

        foreach (var tfm in ExpectedTargetFrameworks)
        {
            using var assembly = ReadEntry(package, $"lib/{tfm}/{id}.dll");
            using var reader = new PEReader(assembly);
            var metadata = reader.GetMetadataReader();

            var bound = metadata.TypeDefinitions
                .Select(metadata.GetTypeDefinition)
                .Count(type =>
                    type.Attributes.HasFlag(TypeAttributes.Public) &&
                    metadata.GetString(type.Namespace).StartsWith(spec.Framework, StringComparison.Ordinal));

            if (PayloadOnly.Contains(id))
            {
                // AppGroupSDK is internal plumbing shipped without a projection; asserting the
                // assembly stays empty is what stops a surface appearing unreviewed.
                Assert.True(
                    bound == 0,
                    $"{id} is meant to ship its xcframework without binding it, but its {tfm} " +
                    $"assembly declares {bound} public {spec.Framework} types.");
                continue;
            }

            Assert.True(
                bound > 0,
                $"{id}'s {tfm} assembly declares no public {spec.Framework} types - " +
                "the binding produced nothing.");
        }
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Package_declares_its_dependency_groups_for_every_target_framework(string id)
    {
        var spec = All.Single(package => package.Id == id);
        var expectedSiblings = spec.DependsOn.OrderBy(dep => dep, StringComparer.Ordinal).ToList();

        using var package = OpenPackage(id);
        var nuspec = ReadNuspec(package, id);

        var groups = nuspec.Descendants()
            .Where(element => element.Name.LocalName == "group")
            .ToList();

        // Asserted per group: the net10 group is grafted in by merge-packages.py from a
        // separately built package, and a missing group there would leave net10 consumers
        // restoring a package whose dependencies never come with it.
        Assert.Equal(
            ExpectedTargetFrameworks.OrderBy(tfm => tfm, StringComparer.Ordinal),
            groups.Select(group => group.Attribute("targetFramework")?.Value ?? string.Empty)
                  .OrderBy(tfm => tfm, StringComparer.Ordinal));

        foreach (var group in groups)
        {
            var siblings = group.Elements()
                .Where(element => element.Name.LocalName == "dependency")
                .Select(element => element.Attribute("id")?.Value ?? string.Empty)
                .Where(dependencyId => dependencyId.StartsWith("SFMCSDK.Net", StringComparison.Ordinal) ||
                                       dependencyId.StartsWith("MarketingCloudSDK.Net", StringComparison.Ordinal))
                .OrderBy(dependencyId => dependencyId, StringComparer.Ordinal)
                .ToList();

            Assert.Equal(expectedSiblings, siblings);
        }
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Package_declares_the_expected_nuspec_metadata(string id)
    {
        var spec = All.Single(package => package.Id == id);
        using var package = OpenPackage(id);
        var nuspec = ReadNuspec(package, id);

        string Value(string element) => nuspec.Descendants()
            .FirstOrDefault(node => node.Name.LocalName == element)?.Value.Trim() ?? string.Empty;

        Assert.Equal(id, Value("id"));
        Assert.NotEmpty(Value("version"));
        Assert.Equal("MIT AND BSD-3-Clause", Value("license"));
        Assert.Equal("icon.png", Value("icon"));
        Assert.Equal("README.md", Value("readme"));
        Assert.Contains(spec.Framework, Value("description"), StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Package_ships_the_icon_readme_and_every_licence_text(string id)
    {
        using var package = OpenPackage(id);

        Assert.True(package.GetEntry("icon.png") is not null, "icon.png is referenced but not packed.");
        Assert.True(package.GetEntry("README.md") is not null, "README.md is referenced but not packed.");

        using var bindings = new StreamReader(ReadEntry(package, "licenses/LICENSE"));
        Assert.Contains("MIT License", bindings.ReadToEnd(), StringComparison.OrdinalIgnoreCase);

        using var native = new StreamReader(ReadEntry(package, "licenses/BSD-3-Clause-Salesforce.txt"));
        Assert.Contains("Salesforce", native.ReadToEnd(), StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Symbol_package_is_produced(string id)
    {
        using var symbols = OpenPackage(id, ".snupkg");

        foreach (var tfm in ExpectedTargetFrameworks)
        {
            Assert.True(
                symbols.GetEntry($"lib/{tfm}/{id}.pdb") is not null,
                $"Symbol package for {id} is missing 'lib/{tfm}/{id}.pdb'.");
        }
    }

    [Fact]
    public void Every_expected_package_was_built_and_nothing_else()
    {
        var found = Directory.GetFiles(ArtifactsDirectory, "*.nupkg")
            .Select(path => Path.GetFileName(path)!)
            .Select(file => file[..FindVersionStart(file)])
            .Distinct()
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToList();

        // The declared SFMC-family dependencies may legitimately be staged into artifacts/ - the
        // local feed is how this repository restores the sibling package before it is on
        // nuget.org; anything else is a stale leftover.
        var allowed = All.SelectMany(spec => spec.DependsOn).Distinct().ToHashSet(StringComparer.Ordinal);

        var expected = All
            .Select(spec => spec.Id)
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToList();

        Assert.Equal(expected, found.Where(id => !allowed.Contains(id) || expected.Contains(id)).ToList());

        static int FindVersionStart(string file)
        {
            for (var i = 0; i < file.Length - 1; i++)
            {
                if (file[i] == '.' && char.IsDigit(file[i + 1]))
                {
                    return i;
                }
            }

            throw new InvalidOperationException($"'{file}' has no version segment.");
        }
    }

    private static string ArtifactsDirectory =>
        Environment.GetEnvironmentVariable("SFMC_ARTIFACTS_DIR") is { Length: > 0 } configured
            ? configured
            : Path.Combine(RepositoryRoot, "artifacts");

    private static string RepositoryRoot
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Directory.Build.props")))
            {
                directory = directory.Parent;
            }

            return directory?.FullName
                ?? throw new InvalidOperationException("Could not locate the repository root.");
        }
    }

    private static PackageSpec[] LoadManifest()
    {
        var path = Path.Combine(RepositoryRoot, "build", "packages.tsv");
        var specs = new List<PackageSpec>();

        foreach (var line in File.ReadAllLines(path))
        {
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var columns = line.Split('\t');
            if (columns.Length < 3)
            {
                continue;
            }

            var dependsOn = columns[2].Trim() is "-" or ""
                ? []
                : columns[2].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            specs.Add(new PackageSpec(columns[0].Trim(), columns[1].Trim(), dependsOn));
        }

        if (specs.Count == 0)
        {
            throw new InvalidOperationException($"No packages were read from {path}.");
        }

        return [.. specs];
    }

    private static ZipArchive OpenPackage(string id, string extension = ".nupkg")
    {
        var matches = Directory.GetFiles(ArtifactsDirectory, $"{id}.*{extension}");

        var package = matches.SingleOrDefault(path =>
            Path.GetFileName(path).StartsWith($"{id}.", StringComparison.Ordinal) &&
            char.IsDigit(Path.GetFileName(path)[id.Length + 1]));

        if (package is null)
        {
            throw new FileNotFoundException(
                $"No {id}{extension} in {ArtifactsDirectory}. Run ./build/BuildNugets.sh first.");
        }

        return ZipFile.OpenRead(package);
    }

    private static MemoryStream ReadEntry(ZipArchive archive, string path)
    {
        var entry = archive.GetEntry(path)
            ?? throw new InvalidOperationException($"Archive has no entry '{path}'.");

        var buffer = new MemoryStream();
        using (var stream = entry.Open())
        {
            stream.CopyTo(buffer);
        }

        buffer.Position = 0;
        return buffer;
    }

    private static XDocument ReadNuspec(ZipArchive package, string id)
    {
        using var stream = ReadEntry(package, $"{id}.nuspec");
        return XDocument.Load(stream);
    }
}
