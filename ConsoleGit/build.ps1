
dotnet publish .\GitAbstraction\GitAbstraction.csproj -c Release -r win-x64
dotnet publish .\ConsoleGit\ConsoleGit.csproj -c Release -r win-x64

$destinationPath = '..\packages\AtfTIDE\Files\exec\'

if (-Not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}
Copy-Item -Path .\ConsoleGit\bin\Release\net9.0\win-x64\publish\* -Destination $destinationPath -Recurse -Force -Exclude "*.pdb"
