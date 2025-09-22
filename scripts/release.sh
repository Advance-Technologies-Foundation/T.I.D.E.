#!/bin/bash
#
# Script for full TIDE release automation.
#
# This script:
#   - Publishes a new release to GitHub
#   - Packs the NuGet package
#   - Publishes the package to NuGet using the version from descriptor.json
#
# Usage:
#   bash release.sh [NuGet API key]
#
# Parameters:
#   [NuGet API key] - (optional) API key for publishing to NuGet. If not provided, publishing may fail.

echo "--- Starting GitHub release ---"
./release-tide-github.sh

echo "--- Packing NuGet package ---"
./pack-nuget-pkg.sh

echo "--- Reading version from descriptor.json ---"
version=$(jq -r '.Descriptor.PackageVersion' ../packages/AtfTIDE/descriptor.json)
echo "version: $version"

echo "--- Publishing TIDE to NuGet ---"
./publish-tide-to-nuget.sh "$version" "$1"