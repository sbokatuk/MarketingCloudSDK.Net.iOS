using System.Text.Json;
using MarketingCloudSDK;
using SFMCSDK;

namespace MarketingCloudSDK.Net.iOS.DeviceTests;

/// <summary>One check: a name and something that throws when the SDK misbehaves.</summary>
public sealed record SmokeTest(string Name, Func<Task> Execute);

/// <summary>
/// Drives the real, packed bindings on a real iOS runtime. No credentials: the configuration below
/// points at no tenant, so nothing registers anywhere - what is being proven is that all three
/// native frameworks (MarketingCloudSDK, its AppGroupSDK payload sibling and the SFMCSDK dependency)
/// load together, that the ObjC surface answers, and that a module-configured initialization is
/// accepted by the core with its module roster structurally intact.
///
/// What these checks deliberately do NOT prove: that the push module finished initializing. Measured
/// on 11.0.2, an unprovisioned tenant leaves every module reading "status": "inactive" and the
/// completion block never fires, and nothing in either state JSON reflects the configuration that
/// was passed - so there is no credential-free assertion for it. Proving that needs a provisioned
/// tenant and an APNs entitlement, which is the manual checklist's job, not CI's.
/// </summary>
public static class SmokeTests
{
    /// <summary>Where progress lines go; the AppDelegate points this at stdout.</summary>
    public static Action<string> Reporter { get; set; } = _ => { };

    private static readonly TaskCompletionSource<SFMCModuleInitStatus[]> Initialized =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public static readonly SmokeTest[] All =
    [
        new("all_three_frameworks_load", () =>
        {
            // Class lookups by ObjC runtime name - a missing or mis-stripped framework fails here
            // with a null class handle rather than three checks later.
            //
            // AppGroupSDK's name comes from its own generated header: AppGroupSDK-Swift.h declares
            // SWIFT_CLASS_NAMED("AppDeviceInfo") @interface SFMCAppDeviceInfo, so the ObjC symbol is
            // real and public even though the .NET binding projects none of it (the package is
            // payload-only). This probe used to guess "AGSAppGroupCoordinator" and, on not finding
            // it, log that the name was not a contract and continue - which meant the framework
            // could have been absent entirely and the check would still have passed.
            foreach (var cls in new[] { "MobilePushSDK", "SFMCSdk", "SFMCAppDeviceInfo" })
            {
                if (ObjCRuntime.Class.GetHandle(cls) == IntPtr.Zero)
                {
                    throw new InvalidOperationException($"Objective-C class '{cls}' is not present.");
                }
            }

            Reporter($"native SFMCSDK {SFMCSdk.SdkVersion} loaded");
            return Task.CompletedTask;
        }),

        new("initialize_with_push_module_is_accepted_by_the_core", async () =>
        {
            // Dummy values shaped like real ones; the SDK accepts them and fails server-side. If
            // the completion block fires (a provisioned environment) that exercises the
            // Action<SFMCModuleInitStatus[]> bridge, and its statuses are asserted; if it does not,
            // see the fallback below for what is left to prove and what is not.
            var pushConfig = new SFMarketingCloudSdkConfigBuilder("00000000-0000-0000-0000-000000000000")
                .SetAccessToken("devicetests-dummy-token")
                .SetMarketingCloudServerUrl(new Foundation.NSUrl("https://localhost.invalid/"))
                .SetMid("000000000")
                .Build();

            var config = new SFMCSdkConfigBuilder()
                .SetPushFeature(pushConfig)
                .Build();

            SFMCSdk.InitializeSdk(config, statuses =>
            {
                foreach (var status in statuses)
                {
                    Reporter($"module status: {status}");
                }

                Initialized.TrySetResult(statuses);
            });

            // Verified against 11.0.2 on a simulator: with an unprovisioned dummy tenant the
            // module's initialization never CONCLUDES (no APNs entitlement, no reachable tenant),
            // so the completion block - which reports concluded statuses - legitimately does not
            // fire. A firing completion (a provisioned environment) is asserted when it happens.
            var completed = await Task.WhenAny(Initialized.Task, Task.Delay(TimeSpan.FromSeconds(20)));
            if (completed == Initialized.Task)
            {
                if (Initialized.Task.Result.Length == 0)
                {
                    throw new InvalidOperationException("The completion block reported zero module statuses.");
                }

                return;
            }

            // The credential-free fallback, and what it is honestly worth. Measured on 11.0.2: the
            // core's module roster reports pushfeature with "status": "inactive" and
            // "version": "unavailable" WHETHER OR NOT a push config was supplied - so no assertion
            // available here can prove the configuration landed. What can be proven is that the
            // core came up and its roster is structurally intact, and that is what this asserts -
            // by parsing, not by substring. A substring check for "pushfeature" (which is what this
            // used to be) passes against the bare stub, so it could not fail for any reason short
            // of the whole state JSON disappearing.
            var state = SFMCSdk.State ?? string.Empty;
            Reporter($"core state after module init: {state}");

            using var document = ParseState(state);

            if (!document.RootElement.TryGetProperty("modules", out var modules)
                || modules.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException(
                    "The core's state JSON has no 'modules' object - its shape changed upstream and "
                    + "everything reading this state (including the MarketingCloudSDK.Net façade's "
                    + "timeout fallback) needs revisiting.");
            }

            if (!modules.TryGetProperty("pushfeature", out var pushFeature)
                || pushFeature.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException(
                    "The core's module roster does not report 'pushfeature' - either the module was "
                    + "renamed upstream or the MobilePush framework is not loaded at all.");
            }

            var status = pushFeature.TryGetProperty("status", out var reported)
                ? reported.GetString()
                : null;

            if (string.IsNullOrEmpty(status))
            {
                throw new InvalidOperationException(
                    "The 'pushfeature' module entry reports no status - the roster's per-module shape changed.");
            }

            // "inactive" is the expected reading here, not a failure: see above. Reported so the log
            // says what was actually observed rather than implying the module came up.
            Reporter($"pushfeature module status: {status} (unprovisioned tenant: 'inactive' is expected)");
        }),

        new("mobile_push_categories_answer", () =>
        {
            var sdk = MobilePushSDK.SharedInstance
                ?? throw new InvalidOperationException("MobilePushSDK.sharedInstance returned null.");

            // Read-side category selectors; none require a registered device.
            Reporter($"push enabled: {sdk.IsPushEnabled}");
            Reporter($"sdk state: {sdk.SDKState ?? "(null)"}");
            return Task.CompletedTask;
        }),
    ];

    /// <summary>
    /// Parses the core's state, turning a non-JSON answer into a failure that says so. The property
    /// is typed as a string and documented as opaque, so "it stopped being JSON" is a real upstream
    /// change rather than an impossible one - and a JsonException with no context would send the
    /// reader looking in the wrong place.
    /// </summary>
    private static JsonDocument ParseState(string state)
    {
        try
        {
            return JsonDocument.Parse(state);
        }
        catch (JsonException error)
        {
            throw new InvalidOperationException(
                $"SFMCSdk.state is not JSON: {error.Message}. Everything that reads this state - "
                + "including the MarketingCloudSDK.Net façade's initialization fallback - assumes it is.",
                error);
        }
    }
}
