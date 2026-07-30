---
applyTo: "src/MarketingCloudSDK.Net.AppGroupSDK.iOS/**"
description: Payload-only AppGroupSDK package.
---

# MarketingCloudSDK.Net.AppGroupSDK.iOS

- This package **ships `AppGroupSDK.xcframework` without projecting any of its API**. It is
  Salesforce-internal plumbing (`app-group-internal-sdk`) that MobilePush v11 loads at runtime; no
  integration document names its types.
- `ApiDefinitions.cs` is comment-only on purpose. It exists because a binding project with no
  `ObjcBindingApiDefinition` does not build. Do not add bindings to it, and do not delete it.
- `PackageLayoutTests` assert this assembly declares **zero** public `AppGroupSDK` types for every
  target framework. A failing "payload-only" assertion means someone added a surface — treat that
  as a design decision needing review, never as a test to relax.
- The project carries no `PackageReference`: it is the leaf of the two-package set, and
  `MarketingCloudSDK.Net.iOS` reaches it by `ProjectReference` so `dotnet pack` emits an
  equal-version dependency.
- Its native line versions independently via `SfmcAppGroupVersion` in `Directory.Build.props`
  (currently 1.0.0), but the **package** version tracks `MarketingCloudSDK.Net.iOS`. Bumping the
  native line means new `build/checksums.txt` pins and a fresh `./build/FetchXcFrameworks.sh`.
- Keep the project a thin shell over `../Sfmc.Binding.props`; anything it needs beyond
  `SfmcFramework`, `SfmcPackageId` and `Description` probably belongs in that shared props file.
