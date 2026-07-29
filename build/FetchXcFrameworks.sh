#!/bin/sh
# Fetches the xcframeworks this repository binds, into ./libs/:
#
#   MarketingCloudSDK.xcframework   from salesforce-marketingcloud/MarketingCloudSDK-iOS
#   AppGroupSDK.xcframework         from salesforce-marketingcloud/app-group-internal-sdk
#
#   ./build/FetchXcFrameworks.sh            # versions from Directory.Build.props
#   ./build/FetchXcFrameworks.sh 11.0.2     # explicit MarketingCloudSDK version
#
# Where they come from: Salesforce commits the built xcframeworks INTO the repository trees and
# attaches nothing to its GitHub releases, so the downloads are tag source archives. The SHA-256
# pins in build/checksums.txt are taken over the FRAMEWORK BINARIES rather than the archives:
# GitHub generates tag archives on the fly and has changed their byte representation before.
set -eu

cd "$(dirname "$0")/.."

prop() {
    sed -n "s/.*<$1>\(.*\)<\/$1>.*/\1/p" Directory.Build.props | head -1
}

MC_VERSION="${1:-$(prop SfmcNativeVersion)}"
AG_VERSION="$(prop SfmcAppGroupVersion)"
CHECKSUMS="build/checksums.txt"

for v in "$MC_VERSION" "$AG_VERSION"; do
    case "$v" in
        *[!0-9.]*|'') echo "error: invalid version '$v'" >&2; exit 1 ;;
    esac
done

work="$(mktemp -d)"
trap 'rm -rf "$work"' EXIT

fetch() {
    # $1 = repo, $2 = version, $3 = path inside the archive, $4 = framework name
    echo "==> downloading $1 v$2"
    curl -fsSL -o "$work/$4.zip" "https://github.com/salesforce-marketingcloud/$1/archive/refs/tags/v$2.zip"
    unzip -q "$work/$4.zip" -d "$work/$4-x"

    framework="$work/$4-x/$1-$2/$3/$4.xcframework"
    if [ ! -d "$framework" ]; then
        echo "error: the $1 v$2 archive has no $3/$4.xcframework - upstream moved it" >&2
        exit 1
    fi

    status=0
    for slice in "$framework"/*/"$4.framework/$4"; do
        rel="$4-$2/$(basename "$(dirname "$(dirname "$slice")")")"
        actual="$(shasum -a 256 "$slice" | cut -d' ' -f1)"
        pinned="$(grep "^$rel " "$CHECKSUMS" | cut -d' ' -f2 || true)"
        if [ -z "$pinned" ]; then
            echo "error: no pin for $rel in $CHECKSUMS (actual: $actual). Run build/UpdateChecksums.sh after a version bump and review the diff." >&2
            status=1
        elif [ "$pinned" != "$actual" ]; then
            echo "error: $rel does not match its pin (pinned: $pinned, actual: $actual) - a tampered or corrupted download, or a stale pin." >&2
            status=1
        else
            echo "==> verified $rel"
        fi
    done
    [ "$status" -eq 0 ] || exit "$status"

    mkdir -p libs
    rm -rf "libs/$4.xcframework"
    mv "$framework" libs/
}

fetch MarketingCloudSDK-iOS "$MC_VERSION" MarketingCloudSDK MarketingCloudSDK
fetch app-group-internal-sdk "$AG_VERSION" AppGroupSDK AppGroupSDK

echo "==> libs/ is ready"
