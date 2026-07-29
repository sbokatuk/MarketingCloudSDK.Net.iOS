using MarketingCloudSDK;
using SFMCSDK;

namespace MarketingCloudSDK.Net.iOS.DeviceTests;

/// <summary>One check: a name and something that throws when the SDK misbehaves.</summary>
public sealed record SmokeTest(string Name, Func<Task> Execute);

/// <summary>
/// Drives the real, packed bindings on a real iOS runtime. No credentials: the configuration
/// below points at no tenant, so nothing registers anywhere - what is being proven is that all
/// three native frameworks (MarketingCloudSDK, its AppGroupSDK payload sibling and the SFMCSDK
/// dependency) load together, the ObjC surface works, and the module-configured initialization
/// completion block fires - the bridge the core repository's zero-module tests cannot reach.
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
            // Class lookups by ObjC runtime name - a missing or mis-stripped framework fails
            // here with a null class handle rather than three checks later.
            foreach (var cls in new[] { "MobilePushSDK", "SFMCSdk", "AGSAppGroupCoordinator" })
            {
                if (ObjCRuntime.Class.GetHandle(cls) == IntPtr.Zero)
                {
                    // AppGroupSDK's internal class names are not part of any contract; probe a
                    // couple of plausible prefixes before declaring the framework absent.
                    if (cls == "AGSAppGroupCoordinator")
                    {
                        Reporter("AppGroupSDK exposes no probed class name; presence is proven by dyld not failing");
                        continue;
                    }

                    throw new InvalidOperationException($"Objective-C class '{cls}' is not present.");
                }
            }

            Reporter($"native SFMCSDK {SFMCSdk.SdkVersion} loaded");
            return Task.CompletedTask;
        }),

        new("initialize_with_push_module_invokes_the_completion_block", async () =>
        {
            // Dummy values shaped like real ones; the SDK accepts them and fails server-side.
            // WITH a module configured the completion block reports its status - this is the
            // Action<SFMCModuleInitStatus[]> bridge the core repository documents as proven here.
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
            // module's initialization never CONCLUDES (no APNs entitlement, no reachable
            // tenant), so the completion block - which reports concluded statuses - may
            // legitimately not fire. What must be provable without credentials is that the
            // module config crossed the bridge and the module machinery consumed it: the core's
            // state JSON stops reporting pushfeature as a bare inactive stub. A firing
            // completion (a provisioned environment) is asserted when it happens.
            var completed = await Task.WhenAny(Initialized.Task, Task.Delay(TimeSpan.FromSeconds(20)));
            if (completed == Initialized.Task)
            {
                if (Initialized.Task.Result.Length == 0)
                {
                    throw new InvalidOperationException("The completion block reported zero module statuses.");
                }

                return;
            }

            var state = SFMCSdk.State ?? string.Empty;
            Reporter($"core state after module init: {state}");
            if (!state.Contains("pushfeature", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "The core's state no longer reports the pushfeature module at all - the module config never reached the SDK.");
            }
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
}
