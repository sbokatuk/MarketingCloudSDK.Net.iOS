#!/bin/sh
# Regenerates build/checksums.txt: one SHA-256 per slice binary of every framework this
# repository binds, at the versions currently pinned in Directory.Build.props.
#
# Run it after a version bump, then diff the result - a hash that CHANGED for an unchanged
# version is exactly the event the pins exist to catch. Pins are over the framework binaries,
# not the tag archives; see FetchXcFrameworks.sh for why.
set -eu

cd "$(dirname "$0")/.."

prop() {
    sed -n "s/.*<$1>\(.*\)<\/$1>.*/\1/p" Directory.Build.props | head -1
}

MC_VERSION="$(prop SfmcNativeVersion)"
AG_VERSION="$(prop SfmcAppGroupVersion)"

work="$(mktemp -d)"
trap 'rm -rf "$work"' EXIT

out="$work/checksums.txt"
cat > "$out" <<'EOF'
# SHA-256 pins for every native framework binary this repository binds.
#
# Verified by build/FetchXcFrameworks.sh before anything lands in libs/. Keys are
# <Framework>-<version>/<xcframework slice>. Regenerate with build/UpdateChecksums.sh after a
# version bump; a hash that changes for an UNCHANGED version is the tampering event these pins
# exist to catch.
EOF

pin() {
    # $1 = repo, $2 = version, $3 = path inside the archive, $4 = framework name
    echo "==> $1 v$2"
    curl -fsSL -o "$work/$4.zip" "https://github.com/salesforce-marketingcloud/$1/archive/refs/tags/v$2.zip"
    unzip -q "$work/$4.zip" -d "$work/$4-x"
    for slice in "$work/$4-x/$1-$2/$3/$4.xcframework"/*/"$4.framework/$4"; do
        rel="$4-$2/$(basename "$(dirname "$(dirname "$slice")")")"
        printf '%s %s\n' "$rel" "$(shasum -a 256 "$slice" | cut -d' ' -f1)" >> "$out"
    done
}

pin MarketingCloudSDK-iOS "$MC_VERSION" MarketingCloudSDK MarketingCloudSDK
pin app-group-internal-sdk "$AG_VERSION" AppGroupSDK AppGroupSDK

mv "$out" build/checksums.txt
echo "==> wrote build/checksums.txt - review the diff before committing"
