# Script to delete specific resource files while preserving en-US
$preserveFile = "data.en-US.json"

# Recursively get all matching files (-Recurse for all subdirectories)
Get-ChildItem -Recurse -Filter "data.*-*.json" |
        Where-Object { $_.Name -ne $preserveFile } |
        ForEach-Object {
            Write-Host "Deleting file: $($_.FullName)"
            Remove-Item $_.FullName
        }

Write-Host "Operation complelsted. All matching files except $preserveFile have been deleted."