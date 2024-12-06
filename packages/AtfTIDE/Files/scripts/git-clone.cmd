@echo off
@setlocal EnableDelayedExpansion

@REM Check if all required parameters are provided
@if "%~1"=="" (
    @echo Usage: %~nx0 repository_url username token destination_path
    @echo Example: %~nx0 tscore-git.creatio.com/k.krylov/lfs.git k.krylov@creatio.com glpat-token C:\path
    @exit /b 1
)
@if "%~2"=="" (
    @echo Error: Username is required
    @exit /b 1
)
@if "%~3"=="" (
    @echo Error: Token is required
    @exit /b 1
)
@if "%~4"=="" (
    @echo Error: Destination path is required
    @exit /b 1
)

@REM Store parameters in variables
@set "REPO_URL=%~1"
@set "USERNAME=%~2"
@set "TOKEN=%~3"
@set "DEST_PATH=%~4"

@REM Create destination directory if it doesn't exist
@if not exist "%DEST_PATH%" mkdir "%DEST_PATH%"

@REM Change to destination directory
@cd /d "%DEST_PATH%"

@REM Extract username without email domain
@for /f "tokens=1 delims=@" %%a in ("%USERNAME%") do @set "USER_ONLY=%%a"

@REM Construct the Git URL with credentials
@set "GIT_URL=https://%USER_ONLY%:%TOKEN%@%REPO_URL%"

@REM Clone the repository
@git clone %GIT_URL% >nul 2>&1 && (
    @echo Repository cloned successfully to %DEST_PATH%
) || (
    @echo Failed to clone repository
    @exit /b 1
)

@endlocal