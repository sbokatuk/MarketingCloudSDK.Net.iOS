using MarketingCloudSDK;
using SFMCSDK;

namespace MarketingCloudSDK.Net.iOS.Example;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnInitializeClicked(object? sender, EventArgs e)
    {
        var appId = AppIdEntry.Text?.Trim();
        var token = AccessTokenEntry.Text?.Trim();
        var serverUrl = ServerUrlEntry.Text?.Trim();
        var mid = MidEntry.Text?.Trim();

        if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(token) ||
            string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(mid))
        {
            AppendLog("Fill in all four MobilePush values first.");
            return;
        }

        InitializeButton.IsEnabled = false;
        AppendLog("Initializing…");

        // The v11 shape: the push module config rides inside the core's module config.
        var pushConfig = new SFMarketingCloudSdkConfigBuilder(appId)
            .SetAccessToken(token)
            .SetMarketingCloudServerUrl(new Foundation.NSUrl(serverUrl))
            .SetMid(mid)
            .Build();

        var config = new SFMCSdkConfigBuilder().SetPushFeature(pushConfig).Build();

        SFMCSdk.InitializeSdk(config, statuses =>
            MainThread.BeginInvokeOnMainThread(() =>
                AppendLog($"Module initialization reported {statuses.Length} status(es).")));

        StatusLabel.Text = $"SDK {SFMCSdk.SdkVersion} initializing.";
        AppendLog("Configuration handed to the SDK.");
        ContactKeyEntry.IsEnabled = true;
        ContactKeyButton.IsEnabled = true;
        StateButton.IsEnabled = true;
    }

    private void OnSetContactKeyClicked(object? sender, EventArgs e)
    {
        var contactKey = ContactKeyEntry.Text?.Trim();
        if (string.IsNullOrEmpty(contactKey))
        {
            AppendLog("Enter a contact key first.");
            return;
        }

        // Contact key is the unified identity's profile id at the v11 generation.
        SFMCSdk.Identity.Edit(modifier =>
        {
            modifier.ProfileId = contactKey;
            return modifier;
        });
        AppendLog($"Contact key set to '{contactKey}'.");
    }

    private void OnShowStateClicked(object? sender, EventArgs e)
    {
        var sdk = MobilePushSDK.SharedInstance;
        AppendLog($"Push enabled: {sdk.IsPushEnabled}");
        AppendLog($"MobilePush state: {sdk.SDKState ?? "(none)"}");
    }

    private void AppendLog(string line)
        => LogLabel.Text = $"{DateTime.Now:HH:mm:ss}  {line}\n{LogLabel.Text}";
}
