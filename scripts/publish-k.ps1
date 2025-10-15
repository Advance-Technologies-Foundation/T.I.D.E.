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

function Update-PackageDescriptor {
    param(
        [Parameter(Mandatory)]
        [string]$DescriptorPath,
        [Parameter(Mandatory)]
        [string]$NewVersion
    )

    if (!(Test-Path -LiteralPath $DescriptorPath -PathType Leaf)) {
        throw "descriptor.json not found: $DescriptorPath"
    }

    Copy-Item -LiteralPath $DescriptorPath -Destination "$DescriptorPath.bak" -Force

    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    $text = [System.IO.File]::ReadAllText($DescriptorPath, $utf8NoBom)

    $m = [regex]::Match($text, '"Descriptor"\s*:\s*\{')
    if (-not $m.Success) { throw "Unexpected descriptor.json format – 'Descriptor' node missing." }

    $start = $m.Index + $m.Length
    $i = $start; $depth = 1
    while ($i -lt $text.Length -and $depth -gt 0) {
        switch ($text[$i]) {
            '{' { $depth++ }
            '}' { $depth-- }
        }
        $i++
    }
    if ($depth -ne 0) { throw "Descriptor block braces are unbalanced." }

    $descBlockLen = ($i - 1) - $start
    $desc = $text.Substring($start, $descBlockLen)

    # значения
    $unixMs = [int64]([DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds())
    $newDate = "\/Date($unixMs)\/"

    $desc = [regex]::Replace($desc, '(?<="PackageVersion"\s*:\s*")([^"]*)', [System.Text.RegularExpressions.MatchEvaluator]{ param($m) $NewVersion }, 1)
    $desc = [regex]::Replace($desc, '(?<="ModifiedOnUtc"\s*:\s*")([^"]*)',   [System.Text.RegularExpressions.MatchEvaluator]{ param($m) $newDate   }, 1)

    $newText = $text.Substring(0, $start) + $desc + $text.Substring($start + $descBlockLen)

    [System.IO.File]::WriteAllText($DescriptorPath, $newText, $utf8NoBom)
}

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

# Build .NET Framework
dotnet build-server shutdown
$build_framework = Join-Path $scriptPath "..\tasks\build-framework.cmd"
& $build_framework

# Build .NET
dotnet build-server shutdown
$build_netcore = Join-Path $scriptPath "..\tasks\build-netcore.cmd"
& $build_netcore

# Set application version and package verison, then publish using Clio
& clio set-app-version "$scriptPath\.." -v $fullver;
# revert this after fix clio set-pkg-version & clio set-pkg-version "$scriptPath\..\packages\AtfTIDE" -v $fullver;
$descriptorPath = Join-Path $scriptPath "..\packages\AtfTIDE\descriptor.json"
Update-PackageDescriptor -DescriptorPath $descriptorPath -NewVersion $fullver

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
