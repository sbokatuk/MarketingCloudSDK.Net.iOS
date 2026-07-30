# MarketingCloudSDK.Net.iOS

## Overview

- This repository publishes **two NuGet packages, versioned together** (currently `11.0.2.2`):
  - `MarketingCloudSDK.Net.iOS` — .NET for iOS bindings for Salesforce's MobilePush framework
    `MarketingCloudSDK.xcframework` 11.0.2 (push registration, inbox, in-app messages, location),
    at the v11 Unified SDK generation.
  - `MarketingCloudSDK.Net.AppGroupSDK.iOS` — **payload-only** carrier of `AppGroupSDK.xcframework`
    1.0.0 (`app-group-internal-sdk`), the internal app-group storage layer v11 requires. Its API is
    deliberately **not** projected.
- `src/MarketingCloudSDK.Net.iOS/ApiDefinitions.cs` (~1,230 lines) and `StructsAndEnums.cs` are
  **hand-maintained**, descended from the 8.x binding and ported to 11.0.2 by header diff.
- v11 surface renames to use and to teach consumers:
  - the `sfmc_*` category surface lives on **`MobilePushSDK`** (was class `MarketingCloudSDK`);
  - configuration is **`SFMarketingCloudSdkConfig`/`SFMarketingCloudSdkConfigBuilder`**, attached with
    `SFMCSdkConfigBuilder.SetPushFeature`, plus `SFMarketingCloudSdk.RequestSdk(Action<IMarketingCloudSdkInterface>)`;
  - URL-handling and event delegates moved to SFMCSDK v4 events; device-token/notification-request
    *setters* left the category surface (readers remain).
- Family: depends on the sibling **`SFMCSDK.Net.iOS`** core package (pinned exact-range
  `[$(SfmcCorePackageVersion)]` = `4.0.1.2`), mirroring the native manifests (`sfmc-sdk-ios ~> 4.0.1`).
  Android is `MarketingCloudSDK.Net.Android`; the cross-platform layer is `MarketingCloudSDK.Net`.

## Build and verify (macOS only)

```sh
./build/FetchXcFrameworks.sh                         # populates ./libs, verifies SHA-256 pins
./build/BuildNugets.sh                               # packs both packages into ./artifacts
dotnet test tests/MarketingCloudSDK.Net.iOS.PackageTests
./.github/scripts/run-simulator-tests.sh 11.0.2.2 net9.0-ios18.0
```

- Requires the .NET 9 **and** .NET 10 SDKs, each with its `ios` workload, and an Xcode from the
  **26.0 line** for the net10 legs. `global.json` pins SDK 9.0.100 (`rollForward: latestFeature`).
- TFMs are `net8.0-ios18.0;net9.0-ios18.0;net10.0-ios26.0`, floor iOS 12.2, selected by the
  `SfmcSdkBand` bands in `src/Sfmc.Binding.props`. No single SDK builds all three, so
  `BuildNugets.sh` packs twice and `build/merge-packages.py` grafts the net10 assets in; payloads
  ship as compressed `.resources.zip`.
- Packing never fetches: run `FetchXcFrameworks.sh` first or the build fails with
  `<Framework>.xcframework is missing`.
- With a newer local Xcode the net10 legs cannot build — run the **net9-band** device tests instead.

## Layout

| Path | What |
| --- | --- |
| `src/MarketingCloudSDK.Net.iOS/` | `ApiDefinitions.cs` + `StructsAndEnums.cs`, hand-maintained |
| `src/MarketingCloudSDK.Net.AppGroupSDK.iOS/` | Payload-only project; comment-only `ApiDefinitions.cs` |
| `src/Sfmc.Binding.props` | TFM bands, xcframework validation, packaging — one-file port from `SFMCSDK.Net.iOS` |
| `build/` | Fetch/checksum/pack/bump scripts, `checksums.txt`, `packages.tsv`, `upstream.tsv` |
| `tests/` | `PackageTests` (in the solution) and `DeviceTests` (simulator app, outside it) |
| `samples/` | MAUI example, consuming packed nupkgs from the local `artifacts/` feed |
| `libs/` | Fetched xcframeworks — gitignored, never committed |
| `docs/release-notes/` | One `<four-part-version>.md` per release |

A gitignored `simulator-tests.log` may sit at the root — ignore it.

## Conventions

- Versions are `<native version>.<binding revision>` from `SfmcNativeVersion` + `SfmcBindingRevision`
  in `Directory.Build.props`. Both packages ship the same version; a binding- or packaging-only
  change advances the revision only.
- Keep property names uniform with the sibling (`SfmcNativeVersion`, not `McNativeVersion`) and keep
  `src/Sfmc.Binding.props` a one-file port with `SFMCSDK.Net.iOS`.
- Keep the core pin **exact-range** `[$(SfmcCorePackageVersion)]`. The Android sibling must use a bare
  pin (Java dependency verification); do not copy that style here.
- The two src projects are linked by **`ProjectReference`**, which `dotnet pack` turns into an
  equal-version NuGet dependency; a `PackageReference` at `$(Version)` could not restore.
- Checksum pins in `build/checksums.txt` are over the **framework slice binaries**, not the tag
  archives (GitHub tag archives are not byte-stable).
- `build/packages.tsv` is the package roster (dependency order: AppGroupSDK first) and
  `build/upstream.tsv` the drift roster; add a package or watched component by editing those, not by
  duplicating lists.
- British spelling in prose and comments ("licence", "behaviour"); API and path names keep their own
  spelling (`licenses/`, `initializeSdk`).
- Licence expression is `MIT AND BSD-3-Clause`; both texts pack from `licenses/`.

## CI and release flow

- `pr.yml` → reusable `build.yml` (`verify: true`) → publishes `-beta.<pr>.<run>` to nuget.org;
  forked PRs build and test but skip publishing (no OIDC token).
- Releasing = **merging `docs/release-notes/<version>.md` to `main`**: `auto-release.yml` tags the
  merge and dispatches `release.yml`, which guards that the tag is an ancestor of the default branch,
  checks the tag against `Directory.Build.props`, packs with `verify: false` and publishes via
  nuget.org **trusted publishing** (OIDC, environment `nuget.org`).
- Release-train order: bump and release `SFMCSDK.Net.iOS` first, then this repository, then the
  umbrella packages.
- `upstream-drift.yml` runs daily; reproduce locally with `DRIFT_DIR=/tmp/d ./build/check-upstream.sh`.
- `build/CheckReadmeVersions.sh` runs in CI: README package pins and the `run-simulator-tests.sh`
  example must match the version being built.

## Testing

- Run `dotnet test tests/MarketingCloudSDK.Net.iOS.PackageTests` for **every** change that packs:
  it asserts per-TFM assemblies, compressed `.resources.zip` payloads, dependency groups, nuspec
  metadata, licence files, symbol packages — and the AppGroupSDK-stays-empty invariant.
- Run the simulator suite when `ApiDefinitions.cs`, packaging or native pins change. It is
  credential-free and uniquely proves that all three frameworks (MarketingCloudSDK + AppGroupSDK +
  SFMCSDK) load together, that the module-configured `initializeSdk` completion bridge is reached,
  and that the `MobilePushSDK` category surface answers.

## Hard rules

- **Never** regenerate `ApiDefinitions.cs` with Objective Sharpie or any generator. Port header
  diffs by hand and keep bidirectional selector parity: every selector in the headers bound, no
  stale selectors left behind.
- **Never** project any AppGroupSDK API. The payload assembly must stay empty of public types;
  adding a surface there is a reviewed design decision, not a fix.
- **Never** commit anything under `libs/` or any native binary. A changed checksum for an unchanged
  version is an incident, not a pin to refresh.
- Keep the `ProjectReference` between the two src projects and the exact-range `SFMCSDK.Net.iOS`
  pin; re-pinning the core means re-verifying `ApiDefinitions.cs` against its surface.
- Pin `SFMCSDK.Net.iOS` at a **stable** version only — never a `-beta.<pr>.<run>` build from its
  `pr.yml`, and never let a prerelease pin reach a release-note merge or a tagged release.
- Version bumps go through `./build/BumpNativeVersion.sh <version>`, then `build/UpdateChecksums.sh`,
  the README pins, and a `docs/release-notes/<version>.md`; bump the core sibling first when
  `sfmc-sdk-ios` moved.
- Release only via the release-note merge → auto-release → guarded `release.yml` chain, with trusted
  publishing. No API keys, no hand-tagging off-branch commits.
- Do not weaken `.github/actions/select-xcode`: the 26.0-line pin and the `Xcode_26.0.1.app` image
  workaround are load-bearing.
- Never commit MobilePush credentials. The sample and tests take them as runtime input, and the
  device tests run credential-free by design.

## References

- Upstream: [`salesforce-marketingcloud/MarketingCloudSDK-iOS`](https://github.com/salesforce-marketingcloud/MarketingCloudSDK-iOS),
  [`salesforce-marketingcloud/app-group-internal-sdk`](https://github.com/salesforce-marketingcloud/app-group-internal-sdk)
- [Salesforce MobilePush module (Unified SDK) documentation](https://developer.salesforce.com/docs/marketing/mobile-unified-sdk/guide)
- Siblings: [`SFMCSDK.Net.iOS`](https://github.com/sbokatuk/SFMCSDK.Net.iOS),
  [`MarketingCloudSDK.Net.Android`](https://github.com/sbokatuk/MarketingCloudSDK.Net.Android),
  [`MarketingCloudSDK.Net`](https://github.com/sbokatuk/MarketingCloudSDK.Net)

Trust these instructions and search the codebase only when something here is incomplete or wrong.
