$env:TIDE_Command="GetDiff";
$env:TIDE_Silent="True";
$env:TIDE_RepoDir="C:\inetpub\wwwroot\clio\tide\Terrasoft.WebApp\conf\tide\testApp";
$env:TIDE_ProcessTimeoutMs="60000"

Start-Process -FilePath "ConsoleGit.exe"