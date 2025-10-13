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
    [ValidatePattern("^\d+\.\d+\.\d+(\.\d+)?$")]
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

& clio set-app-version "$scriptPath\.." -v $fullver;
& clio set-pkg-version "$scriptPath\..\packages\AtfTIDE" -v $fullver;
& clio publish-app --app-name AtfTide --app-version $fullver --app-hub "$scriptPath\..\Artifacts" --repo-path "$scriptPath\..";


# Add cliogate.gz to AtfTide_fullver.zip
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zipPath = Join-Path $scriptPath "\..\Artifacts\AtfTide\$fullver\AtfTide_$fullver.zip"
$gzPath = Join-Path $scriptPath "\..\cliogate\cliogate.gz"
if ((Test-Path $zipPath -PathType Leaf) -and (Test-Path $gzPath -PathType Leaf)) {
    Add-Type -AssemblyName System.IO.Compression
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::Open($zipPath, [System.IO.Compression.ZipArchiveMode]::Update)
    try {
        $entry = $zip.GetEntry("cliogate.gz")
        if ($entry) { $entry.Delete() }
        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $gzPath, "cliogate.gz")
    } finally {
        $zip.Dispose()
    }
} else {
    Write-Error "Either zip file or cliogate.gz not found."
}
