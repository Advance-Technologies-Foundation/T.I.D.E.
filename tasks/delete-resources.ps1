
<#
.SYNOPSIS
Deletes resource XML files except for the en-US version.

.DESCRIPTION
This script recursively searches for files matching the pattern 'resource.*-*.xml' in the parent directory of the script location and its subdirectories. It deletes all matching files except 'resource.en-US.xml', which is preserved.

.EXAMPLE
PS> .\delete-resources.ps1

Deletes all resource XML files except 'resource.en-US.xml' in the workspace.

#>


# Change to the parent directory of the script location
Set-Location (Split-Path $MyInvocation.MyCommand.Path -Parent | Split-Path -Parent)

# Script to delete specific resource files while preserving en-US
$preserveFile = "resource.en-US.xml"

# Recursively get all matching files (-Recurse for all subdirectories)
Get-ChildItem -Recurse -Filter "resource.*-*.xml" |
        Where-Object { $_.Name -ne $preserveFile } |
        ForEach-Object {
            Write-Host "Deleting file: $($_.FullName)"
            Remove-Item $_.FullName
        }

Write-Host "Operation completed. All matching files except $preserveFile have been deleted."