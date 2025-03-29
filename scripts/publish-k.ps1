<#
.SYNOPSIS
    Publishes the AtfTide application with versioning.

.DESCRIPTION
    This script publishes the AtfTide application, first cleaning up data and resources,
    then determining the version (either from parameter or using GitVersion),
    and finally publishing the application using Clio.

.PARAMETER version
    Optional. The version to use for publishing. If not provided, version will be
    determined using GitVersion.

.EXAMPLE
    .\publish-k.ps1
    # Uses GitVersion to determine the version

.EXAMPLE
    .\publish-k.ps1 -version "1.2.3"
    # Uses the specified version "1.2.3"
#>
param(
    [Parameter(Mandatory=$false)][alias("v")]
    [ValidatePattern("^\d+\.\d+\.\d+(\.\d+)?$", ErrorMessage="Version must be in format: Major.Minor.Patch[.Build]")]
    [string]$version
)

# Remove non en-US data
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
& "$scriptPath\delete-data.ps1"

# Remove non en-US Resources
& "$scriptPath\delete-resources.ps1"


if (-not $version) {
    $version = dotnet gitversion /output json /showvariable MajorMinorPatch;
    $build = dotnet gitversion /output json /showvariable CommitsSinceVersionSource;
    $fullver = "$version.$build";
} else {
    $fullver = $version;
}

clio publish-app --app-name AtfTide --app-version $fullver --app-hub .\Artifacts --repo-path .;


