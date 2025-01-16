call set-version.cmd %1
call pack-nuget-pkg.cmd
call publish-tide.cmd %1 %2