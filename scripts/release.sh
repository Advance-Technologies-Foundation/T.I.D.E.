#!/bin/bash
./release-tide-github.sh

./pack-nuget-pkg.sh
# Read version from descriptor.json
echo "--- Starting GitHub release ---"
./release-tide-github.sh

echo "--- Packing NuGet package ---"
./pack-nuget-pkg.sh

echo "--- Reading version from descriptor.json ---"
version=$(jq -r '.Descriptor.PackageVersion' ../packages/AtfTIDE/descriptor.json)
echo "NuGet publish version: $version"

echo "--- Publishing TIDE to NuGet ---"
./publish-tide-to-nuget.sh "$version" "$1"