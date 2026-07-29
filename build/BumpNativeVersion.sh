#!/bin/sh
# Runs every scripted step of a MarketingCloudSDK upgrade, in order, stopping at the first
# failure.
#
#   ./build/BumpNativeVersion.sh 11.1.0
#
# AppGroupSDK versions independently (SfmcAppGroupVersion): read the new version's package
# manifest first and bump that property by hand if it moved. The SFMCSDK.Net.iOS pin
# (SfmcCorePackageVersion) usually moves in the same event; bump the sibling repository first.
#
# What it does NOT automate:
#   - reviewing the checksum diff;
#   - the ApiDefinitions port: diff the new tag's headers against the previous ones and
#     hand-apply the changes (never regenerate - the file is hand-curated);
#   - the README's version pins (build/CheckReadmeVersions.sh will tell you);
#   - a release-notes file under docs/release-notes/;
#   - bumping SfmcBindingRevision back to 1 for the new native line.
set -eu

version="$1"

case "$version" in
    *[!0-9.]*|'')
        echo "usage: $0 <marketingcloudsdk-version>" >&2
        exit 1
        ;;
esac

root="$(cd "$(dirname "$0")/.." && pwd)"
props="$root/Directory.Build.props"

echo "==> pinning SfmcNativeVersion $version"
sed -i '' "s:<SfmcNativeVersion>.*</SfmcNativeVersion>:<SfmcNativeVersion>$version</SfmcNativeVersion>:" "$props"

echo "==> regenerating framework checksums"
"$root/build/UpdateChecksums.sh"

echo "==> fetching the new xcframeworks"
"$root/build/FetchXcFrameworks.sh" "$version"

echo "==> packing"
"$root/build/BuildNugets.sh"

echo "==> package tests"
dotnet test "$root/tests/MarketingCloudSDK.Net.iOS.PackageTests"

echo "==> done. Review the checksum diff, port the header changes into ApiDefinitions.cs,"
echo "    update the README and docs/release-notes/ - this script does not do that for you."
