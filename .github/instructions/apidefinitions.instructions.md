---
applyTo: "src/MarketingCloudSDK.Net.iOS/*.cs"
description: Hand-maintained MobilePush binding definitions.
---

# ApiDefinitions.cs and StructsAndEnums.cs

- These files are **hand-curated**. Never regenerate them with Objective Sharpie or any other
  generator, and never paste generator output over them: the naming, the retained comments and the
  8.x-derived member shapes are deliberate.
- Update by **header diff**: compare the new tag's headers (the ObjC `MobilePushSDK` categories and
  `MarketingCloudSDK-Swift.h`) against the previous tag's and hand-apply the changes.
- Keep **bidirectional selector parity**: every selector declared in the headers has a binding, and
  no binding names a selector that no longer exists. Both directions are the check — an unbound
  selector is a gap, a stale one is a runtime crash.
- Keep the `// -(...)` / `// @interface ...` header comment above each member and type; it is what
  makes the next diff reviewable.
- Use the v11 names: the category surface is `MobilePushSDK` (with `MobilePushSDK_*` category
  interfaces), configuration is `SFMarketingCloudSdkConfig`/`SFMarketingCloudSdkConfigBuilder`, and
  the accessor is `SFMarketingCloudSdk.RequestSdk`. Do not reintroduce the 8.x `MarketingCloudSDK`
  class, `PushConfig`/`PushConfigBuilder` or `SFMCSdkPushModule`.
- Do **not** bind types owned by the core framework (module/subscriber protocols,
  `SFMCSdkComponents`, event classes, shared enums, the delegate protocols that moved at v4). They
  come from the sibling `SFMCSDK.Net.iOS` package via the `SFMCSDK` namespace. If a needed type is
  missing there, fix the sibling and re-pin — do not duplicate it here.
- The namespace stays `MarketingCloudSDK` (it is the packages' `RootNamespace`), even though the
  main class is now `MobilePushSDK`.
- Enum members in `StructsAndEnums.cs` keep their native casing (`configureNoError`); do not
  "tidy" them into .NET casing — that is a breaking change for consumers.
- Any change here is an API change: re-pack, run `dotnet test tests/MarketingCloudSDK.Net.iOS.PackageTests`,
  and run `./.github/scripts/run-simulator-tests.sh <version> net9.0-ios18.0`. Note removals and
  renames in the release note.
