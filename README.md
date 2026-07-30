# MarketingCloudSDK.Net.iOS

[![NuGet](https://img.shields.io/nuget/v/MarketingCloudSDK.Net.iOS?label=nuget)](https://www.nuget.org/packages/MarketingCloudSDK.Net.iOS)
[![release](https://github.com/sbokatuk/MarketingCloudSDK.Net.iOS/actions/workflows/release.yml/badge.svg)](https://github.com/sbokatuk/MarketingCloudSDK.Net.iOS/actions/workflows/release.yml)
[![Targets: net8.0 | net9.0 | net10.0](https://img.shields.io/badge/targets-net8.0%20%7C%20net9.0%20%7C%20net10.0-512BD4)](#installing)
[![MarketingCloudSDK 11.0.2](https://img.shields.io/badge/MarketingCloudSDK-11.0.2-099DFD)](https://github.com/salesforce-marketingcloud/MarketingCloudSDK-iOS/releases)
[![Licence: MIT AND BSD-3-Clause](https://img.shields.io/badge/licence-MIT%20AND%20BSD--3--Clause-orange)](#licence)

**.NET for iOS bindings for the Salesforce Marketing Cloud MobilePush framework (`MarketingCloudSDK`) — push registration, inbox, in-app messages and location messaging — at the v11 Unified SDK generation.**

```sh
dotnet add package MarketingCloudSDK.Net.iOS
```

```csharp
using Foundation;
using MarketingCloudSDK;
using SFMCSDK;

var pushConfig = new SFMarketingCloudSdkConfigBuilder("your-app-id")
    .SetAccessToken("your-access-token")
    // NSUrl, not a string - the builder takes the native type the header declares.
    .SetMarketingCloudServerUrl(new NSUrl("https://your-tenant.device.marketingcloudapis.com/"))
    .SetMid("your-mid")
    .Build();

var config = new SFMCSdkConfigBuilder().SetPushFeature(pushConfig).Build();
SFMCSdk.InitializeSdk(config, statuses => { });

// Contact key at v11 is a core identity edit, not a module call:
SFMCSdk.Identity.Edit(modifier => { modifier.ProfileId = "contact-key"; return modifier; });

// The sfmc_* category surface lives on MobilePushSDK (renamed from MarketingCloudSDK at v11).
// Selectors keep their sfmc_ prefix natively; the binding projects them without it:
var sdk = MobilePushSDK.SharedInstance;
var pushEnabled = sdk.IsPushEnabled;
```

The SFMC SDK core (`SFMCSDK.Net.iOS`) and the AppGroupSDK payload (`MarketingCloudSDK.Net.AppGroupSDK.iOS`) arrive as dependencies of this package, mirroring the native package manifests. Android lives in the sibling [MarketingCloudSDK.Net.Android](https://github.com/sbokatuk/MarketingCloudSDK.Net.Android); a cross-platform ergonomic layer lives in [MarketingCloudSDK.Net](https://github.com/sbokatuk/MarketingCloudSDK.Net).

---

## Contents

- [Packages](#packages)
- [What is bound, and how](#what-is-bound-and-how)
- [Installing](#installing)
- [Push prerequisites](#push-prerequisites)
- [How this repository works](#how-this-repository-works)
- [Building locally](#building-locally)
- [Tests](#tests)
- [Upgrading the SFMC SDK](#upgrading-the-sfmc-sdk)
- [Releasing](#releasing)
- [Troubleshooting](#troubleshooting)
- [Licence](#licence)

## Packages

| Package | Wraps | Depends on | What it is for |
| --- | --- | --- | --- |
| `MarketingCloudSDK.Net.iOS` | `MarketingCloudSDK.xcframework` 11.0.2 | `SFMCSDK.Net.iOS`, `MarketingCloudSDK.Net.AppGroupSDK.iOS` | Push registration, inbox, in-app messages, location |
| `MarketingCloudSDK.Net.AppGroupSDK.iOS` | `AppGroupSDK.xcframework` 1.0.0 | — | Payload-only: the internal app-group storage layer v11 requires; no API projected |

Versions are `<MarketingCloudSDK version>.<binding revision>` — `11.0.2.3` is MarketingCloudSDK 11.0.2, binding revision 3. Both packages version together; AppGroupSDK's own native line (1.0.0) is pinned in `Directory.Build.props`.

## What is bound, and how

**By hand, never by Sharpie**, descending from the hand-curated 8.x binding, ported to 11.0.2 by header diff and verified with a bidirectional selector-parity check (every selector in all ten headers bound, no stale selectors). The packed binding passes live smoke tests on a simulator — all three frameworks load together and a module-configured initialization is accepted by the core with its module roster intact.

What changed at v11, for consumers of the old 8.x packages:

- The `sfmc_*` category surface moved from class `MarketingCloudSDK` to **`MobilePushSDK`** — same selectors, new class name.
- `PushConfig`/`PushConfigBuilder`/`SFMCSdkPushModule` are replaced by **`SFMarketingCloudSdkConfig(Builder)`** attached via `SFMCSdkConfigBuilder.SetPushFeature`, and **`SFMarketingCloudSdk`** with `RequestSdk(Action<IMarketingCloudSdkInterface>)`.
- The URL-handling and event delegates left this framework (v4 SFMCSDK carries event-based replacements — `SFMCSdkPushURLHandlingEvent` and friends — and the new `sfmc_handle*Event:` hooks here feed them).
- Device-token and notification-request *setters* left the category surface (the SDK observes them itself); the readers remain.

## Installing

```xml
<PackageReference Include="MarketingCloudSDK.Net.iOS" Version="11.0.2.3" />
```

Target frameworks: `net8.0-ios18.0`, `net9.0-ios18.0`, `net10.0-ios26.0`. Floor: **iOS 12.2**.

## Push prerequisites

The binding does not change what MobilePush itself needs: an APNs-entitled app (`aps-environment`), a `UNUserNotificationCenter` authorization request, `RegisterForRemoteNotifications`, and MobilePush app credentials. Never commit credentials; the sample takes them as runtime input.

## How this repository works

Nothing native is committed. Salesforce commits the built xcframeworks into the repository trees (`MarketingCloudSDK-iOS`, `app-group-internal-sdk`) and attaches nothing to releases, so [build/FetchXcFrameworks.sh](build/FetchXcFrameworks.sh) downloads both tag archives and verifies every slice binary against the SHA-256 pins in [build/checksums.txt](build/checksums.txt) — pins over binaries, not archives, because GitHub tag archives are not byte-stable.

[build/BuildNugets.sh](build/BuildNugets.sh) packs twice (net9 band, then net10 from a scratch `global.json`) and [build/merge-packages.py](build/merge-packages.py) grafts the net10 assets into each package. The native payloads ship as compressed `.resources.zip`.

### Layout

| Path | What |
| --- | --- |
| `src/Sfmc.Binding.props` | TFM bands, xcframework validation, packaging — a one-file port from the sibling SFMCSDK.Net.iOS |
| `src/MarketingCloudSDK.Net.iOS/` | `ApiDefinitions.cs` + `StructsAndEnums.cs`, hand-maintained |
| `src/MarketingCloudSDK.Net.AppGroupSDK.iOS/` | The payload-only sibling; its ApiDefinitions deliberately binds nothing |
| `build/` | Fetch, checksum, pack and upgrade scripts; `packages.tsv` is the roster |
| `tests/` | `PackageTests` and `DeviceTests` (a bare UIKit app driving the real SDK on a simulator) |
| `samples/` | A MAUI app driving configuration and registration |
| `.github/workflows/` | `pr`, `release`, `auto-release`, `upstream-drift` |

## Building locally

```sh
./build/FetchXcFrameworks.sh          # populate ./libs, checksum-verified
./build/BuildNugets.sh                # packs 11.0.2.3 into ./artifacts
dotnet test tests/MarketingCloudSDK.Net.iOS.PackageTests
```

`SFMCSDK.Net.iOS` restores from nuget.org, or from `./artifacts` if you drop the sibling's package there.

## Tests

```sh
dotnet test tests/MarketingCloudSDK.Net.iOS.PackageTests
./.github/scripts/run-simulator-tests.sh 11.0.2.3 net9.0-ios18.0
```

The simulator tests run without credentials on purpose: all three frameworks load (each probed by a real exported ObjC class name), a module-configured `initializeSdk` is accepted and the core's module roster still reports `pushfeature` with a parseable per-module shape, and the `MobilePushSDK` category surface answers.

What they cannot prove, and do not claim to: that the push module finished initializing. Measured on 11.0.2, an unprovisioned tenant leaves every module reading `"status": "inactive"` and the completion block never fires — and nothing in either state JSON reflects the configuration that was passed, so no credential-free assertion exists for it. That belongs to the manual checklist with a provisioned tenant and an APNs entitlement.

## Upgrading the SFMC SDK

```sh
./build/BumpNativeVersion.sh 11.1.0
```

then port the header diff into `ApiDefinitions.cs` by hand — the script prints what it does not automate. Bump the sibling `SFMCSDK.Net.iOS` first when `sfmc-sdk-ios` moved; `upstream-drift` watches both repositories this package binds, and the sibling's package on nuget.org — a core release lands here as a re-pin of `SfmcCorePackageVersion` in `Directory.Build.props` and a binding-revision release.

## Releasing

Merging a file named `docs/release-notes/<four-part-version>.md` to `main` **is** the release: `auto-release` tags it, `release` packs with verification off and publishes to nuget.org via trusted publishing. Every PR publishes a `-beta.<pr>.<run>` prerelease.

## Troubleshooting

**`MarketingCloudSDK.xcframework is missing`.** Run `./build/FetchXcFrameworks.sh` — nothing native is committed, and packing does not fetch.

**Migrating 8.x code: `MarketingCloudSDK` (the class) does not exist.** It is `MobilePushSDK` at v11 — the `sfmc_*` members are unchanged. See [What is bound, and how](#what-is-bound-and-how) for the config-builder replacement.

**Xcode version errors building the net10 sample locally.** The net10 iOS workload pins an exact Xcode line (CI selects `Xcode_26.0`); with a newer local Xcode, build the net9-band device tests instead.

## Licence

The binding code in this repository is [MIT](LICENSE). The native frameworks inside the packages are © Salesforce, [BSD-3-Clause](licenses/BSD-3-Clause-Salesforce.txt). The package licence expression is `MIT AND BSD-3-Clause` and both texts ship in every package under `licenses/`.
