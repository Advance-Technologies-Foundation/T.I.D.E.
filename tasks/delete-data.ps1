<#
.SYNOPSIS
Deletes resource files matching 'data.*-*.json' except 'data.en-US.json' from the script's parent directory and its subdirectories.

.DESCRIPTION
This script recursively searches for files named 'data.*-*.json' (e.g., data.fr-FR.json, data.es-ES.json) in the parent directory of the script and all its subdirectories. It deletes all matching files except 'data.en-US.json', which is preserved.

.EXAMPLE
PS> .\delete-data.ps1

Deletes all resource files except 'data.en-US.json' from the relevant directories.
#>

# Change to the parent directory of the script location
Set-Location (Split-Path $MyInvocation.MyCommand.Path -Parent | Split-Path -Parent)

# Script to delete specific resource files while preserving en-US
$preserveFile = "data.en-US.json"

# Recursively get all matching files (-Recurse for all subdirectories)
Get-ChildItem -Recurse -Filter "data.*-*.json" |
        Where-Object { $_.Name -ne $preserveFile } |
        ForEach-Object {
            Write-Host "Deleting file: $($_.FullName)"
            Remove-Item $_.FullName
        }

Write-Host "Operation completed. All matching files except $preserveFile have been deleted."