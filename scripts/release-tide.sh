#!/bin/bash
# Script for releasing TIDE on GitHub

set -e
set -x



echo "Getting the latest release version..."
gh_release_output=$(gh release list --limit 1 2>&1)
if [ $? -ne 0 ]; then
  echo "Warning: gh release list failed. Output: $gh_release_output"
  last_version="1.0.0"
else
  # Get tag from first column
  last_tag=$(echo "$gh_release_output" | awk '{print $1}' | grep -oE 'v[0-9]+\.[0-9]+\.[0-9]+|[0-9]+\.[0-9]+\.[0-9]+')
  if [ -z "$last_tag" ]; then
    echo "No previous releases found. Using default version 1.0.0."
    last_version="1.0.0"
  else
    last_version=$(echo "$last_tag" | sed 's/^v//')
    echo "Latest version: $last_version"
  fi
fi



echo "Calculating new version..."

major=$(echo $last_version | cut -d. -f1)
minor=$(echo $last_version | cut -d. -f2)
patch=$(echo $last_version | cut -d. -f3)

release_type=${1:-patch}

case "$release_type" in
  major)
    major=$((major+1))
    minor=0
    patch=0
    ;;
  minor)
    minor=$((minor+1))
    patch=0
    ;;
  patch)
    patch=$((patch+1))
    ;;
  *)
    echo "Unknown release type: $release_type. Use major, minor, or patch."
    exit 1
    ;;
esac

new_version="$major.$minor.$patch"
echo "New version: $new_version (type: $release_type)"



archive_name="../publish/TIDE_${new_version}.zip"
echo "Archive name: $archive_name"



echo "Compressing TIDE using clio..."
clio compress ../packages/AtfTIDE -d ../publish/AtfTIDE${new_version}.gz



echo "Checking if ../publish/AtfTIDE${new_version}.gz exists and is not empty..."
if [ ! -s ../publish/AtfTIDE${new_version}.gz ]; then
  echo "Error: ../publish/AtfTIDE${new_version}.gz does not exist or is empty."
  exit 1
fi


echo "Checking if ../cliogate.gz exists and is not empty..."
if [ ! -s ../cliogate.gz ]; then
  echo "Error: ../cliogate.gz does not exist or is empty."
  exit 1
fi




echo "Adding AtfTIDE${new_version}.gz and cliogate.gz to zip archive..."
cp ../cliogate.gz ../publish/cliogate.gz
cd ../publish
zip "TIDE_${new_version}.zip" "AtfTIDE${new_version}.gz" "cliogate.gz"
cd -


echo "Checking resulting archive for consistency..."
if [ ! -s $archive_name ]; then
  echo "Error: $archive_name was not created or is empty."
  exit 1
fi
unzip -l $archive_name
echo "Archive $archive_name contents listed above."

gh release create "$new_version" $archive_name --title "TIDE $new_version" --notes "Automated TIDE release version $new_version"
echo "Release $new_version created and archive $archive_name attached."

echo "Creating release and attaching archive..."
gh release create "$new_version" $archive_name --title "TIDE $new_version" --notes "Automated TIDE release version $new_version"

echo "Release $new_version created and archive $archive_name attached."
