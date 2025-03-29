# Script to delete specific resource files while preserving en-US

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$preserveFile = "resource.en-US.xml"

# Recursively get all matching files (-Recurse for all subdirectories)
Get-ChildItem -Recurse -Path "$scriptPath\..\packages\AtfTIDE\Resources" -Filter "resource.*-*.xml" |
        Where-Object { $_.Name -ne $preserveFile } |
        ForEach-Object {
            Write-Host "Deleting file: $($_.FullName)"
            Remove-Item $_.FullName
        }

Write-Host "Operation complelsted. All matching files except $preserveFile have been deleted."