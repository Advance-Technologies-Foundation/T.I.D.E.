#!/bin/bash

dotnet nuget push "../publish/AtfTIDE.$1.nupkg" --api-key "$2" --source "https://api.nuget.org/v3/index.json"