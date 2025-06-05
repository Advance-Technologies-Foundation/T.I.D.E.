#dotnet publish .\ConsoleGit\ConsoleGit.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish .\ConsoleGit\ConsoleGit.csproj -c Release
#dotnet publish .\ConsoleGit\ConsoleGit.csproj -c Release -r win-x64 -p:PublishSingleFile=true

#dotnet publish .\GitAbstraction\GitAbstraction.csproj -c Release -r win-x64
#dotnet publish .\ConsoleGit\ConsoleGit.csproj -c Release -r win-x64

$destinationPath = '..\packages\AtfTIDE\Files\exec\'

if (-Not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}
#Copy-Item -Path .\ConsoleGit\bin\Release\net9.0\win-x64\publish\* -Destination $destinationPath -Recurse -Force -Exclude "*.pdb"

Get-ChildItem -Path $destinationPath -Recurse | Remove-Item -Recurse -Force
Copy-Item -Path .\ConsoleGit\bin\Release\net8.0\publish\* -Destination $destinationPath -Recurse -Force -Exclude "*.pdb"

#
Compress-Archive -Path $destinationPath\* -DestinationPath "$destinationPath\archive.zip" -Force
Get-ChildItem -Path $destinationPath -File | Where-Object { $_.Name -ne 'archive.zip' } | Remove-Item -Force
Get-ChildItem -Path $destinationPath -Directory | Remove-Item -Recurse -Force
