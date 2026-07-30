---
applyTo: ".github/workflows/*.yml,.github/actions/**"
description: CI invariants for the iOS binding pipeline.
---

# Workflows and composite actions

- Every job that touches the native toolchain runs on **`macos-15`** and must call
  `./.github/actions/select-xcode` before `dotnet build`/`pack`/`test`. Do not relax the action's
  `26.0` iOS SDK line, its newest-patch-first ordering (the `Xcode_26.0.app` alias on the image is
  incomplete; `Xcode_26.0.1.app` is the working bundle), or its `xcodebuild -find install_name_tool`
  probe — `xcode-select -s` fails silently on a bad path and the breakage surfaces jobs later.
- `build.yml` is the reusable pipeline. Its `verify` input is the gate on package validation, the
  sample build, the Release device link check and the simulator suite: pull requests leave it
  `true`, releases pass `false` because the tagged commit was verified on its pull request. Keep
  the input's name and meaning identical across the sibling repositories.
- Both SDK bands must be installed where they are used: `global.json` pins .NET 9, so anything
  needing .NET 10 is invoked from a scratch directory carrying its own `global.json` (the SDK is
  resolved from the working directory). Follow that pattern rather than adding SDK-switching hacks.
- Keep the xcframework cache key bound to both the native version and
  `hashFiles('build/FetchXcFrameworks.sh', 'build/checksums.txt')`, so re-pinning a hash invalidates
  the cache.
- Publishing uses nuget.org **trusted publishing**: the job needs `id-token: write`, the
  `nuget.org` environment, and `NuGet/login@v1` immediately before `dotnet nuget push` (the issued
  key lasts an hour and each OIDC token exchanges once). Never add a stored API key.
- Keep `release.yml`'s guard job (tag must be an ancestor of the default branch) and its
  tag-versus-`Directory.Build.props` check. Keep `auto-release.yml` triggered on the push to `main`
  with `--diff-filter=A`, and keep its `workflow_dispatch` of `release.yml` — a tag pushed with
  `GITHUB_TOKEN` does not trigger `on: push: tags`.
- Keep the forked-PR publish skip in `pr.yml`: forks get no OIDC token for this repository.
- Generate package lists from `build/packages.tsv` rather than hardcoding ids, and keep
  `./build/CheckReadmeVersions.sh` in the pack job.
- Grant the narrowest `permissions:` block a job needs, and pin actions by major tag as the existing
  workflows do.
