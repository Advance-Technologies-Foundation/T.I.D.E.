#!/bin/bash

./set-version.sh "$1"
./pack-nuget-pkg.sh
./publish-tide.sh "$1" "$2"