$env:CoreLibPath="..\..\..\.application\net-framework\core-bin"
$env:TestCoreLibPath="..\..\.application\net-framework\core-bin"
$env:RelativePkgFolderPath="..\..\..\.application\net-framework\packages"
$env:CoreTargetFramework="net472"

Remove-Item -Path "..\documentation\_site" -Recurse -Force -ErrorAction SilentlyContinue

docfx metadata ../documentation/docfx.json --log log.json;
docfx build ../documentation/docfx.json --maxParallelism 16;
docfx serve ../documentation/_site --open-browser