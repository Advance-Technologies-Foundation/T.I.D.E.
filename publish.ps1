.\delete-data.ps1;
.\delete-resources.ps1
$version = dotnet gitversion /output json /showvariable MajorMinorPatch;
$build = dotnet gitversion /output json /showvariable CommitsSinceVersionSource;

$fullver = "$version.$build";

clio publish-app --app-name AtfTide --app-version $fullver --app-hub .\Artifact --repo-path .;