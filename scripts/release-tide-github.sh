#!/bin/bash
# Script for releasing TIDE on GitHub

set -e



gh_release_output=$(gh release list --limit 1 2>&1)
if [ $? -ne 0 ]; then
  last_version="1.0.0"
else
  # Get tag from first column
  last_tag=$(echo "$gh_release_output" | awk '{print $4}' | grep -oE 'v[0-9]+\.[0-9]+\.[0-9]+|[0-9]+\.[0-9]+\.[0-9]+')
  if [ -z "$last_tag" ]; then
    last_version="1.0.0"
  else
    last_version=$(echo "$last_tag" | sed 's/^v//')
  fi
fi




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


# Set new version in project files
../scripts/set-version.sh "$new_version"

# Commit and push changes to GitHub
git add -A
git commit -m "chore: set version $new_version"
git push origin main


rm -rf ../publish/*

archive_name="../publish/TIDE_${new_version}.zip"



clio compress ../packages/AtfTIDE -d ../publish/AtfTIDE${new_version}.gz



if [ ! -s ../publish/AtfTIDE${new_version}.gz ]; then
  echo "Error: ../publish/AtfTIDE${new_version}.gz does not exist or is empty."
  exit 1
fi


if [ ! -s ../cliogate.gz ]; then
  echo "Error: ../cliogate.gz does not exist or is empty."
  exit 1
fi




cp ../cliogate.gz ../publish/cliogate.gz
cd ../publish
zip "TIDE_${new_version}.zip" "AtfTIDE${new_version}.gz" "cliogate.gz"
cd -


if [ ! -s $archive_name ]; then
  echo "Error: $archive_name was not created or is empty."
  exit 1
fi
unzip -l $archive_name


# Create release only if tag does not exist
if gh release view "$new_version" > /dev/null 2>&1; then
  echo "Release with tag $new_version already exists. Skipping release creation."
else
  gh release create "$new_version" $archive_name --title "TIDE $new_version" --notes "Automated TIDE release version $new_version"
fi

