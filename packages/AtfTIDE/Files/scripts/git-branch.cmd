@REM Usage: git-branch.cmd C:\2\lfs feature/my-feature
@REM Usage: git-branch.cmd C:\2\lfs feature/my-feature main

@echo off
@setlocal EnableDelayedExpansion

@REM Check if all required parameters are provided
@if "%~1"=="" (
    @echo Usage: %~nx0 git_directory branch_name [base_branch]
    @echo Example: %~nx0 C:\myrepo feature/my-feature main
    @exit /b 1
)
@if "%~2"=="" (
    @echo Error: Branch name is required
    @exit /b 1
)

@REM Store parameters in variables
@set "GIT_DIR=%~1"
@set "NEW_BRANCH=%~2"
@set "BASE_BRANCH=%~3"

@REM Check if directory exists
@if not exist "%GIT_DIR%" (
    @echo Error: Directory '%GIT_DIR%' does not exist
    @exit /b 1
)

@REM Change to git directory
@pushd "%GIT_DIR%" || (
    @echo Error: Failed to change to directory '%GIT_DIR%'
    @exit /b 1
)

@REM Check if .git directory exists
@if not exist ".git" (
    @echo Error: '%GIT_DIR%' is not a git repository ^(.git folder not found^)
    @popd
    @exit /b 1
)

@REM If base branch is not specified, use current branch
@if "%BASE_BRANCH%"=="" (
    @for /f "tokens=* USEBACKQ" %%a in (`git rev-parse --abbrev-ref HEAD`) do @set "BASE_BRANCH=%%a"
)

@REM Fetch latest changes
@git fetch >nul 2>&1 || (
    @echo Error: Failed to fetch latest changes
    @popd
    @exit /b 1
)

@REM Check if branch already exists
@git show-ref --verify --quiet refs/heads/%NEW_BRANCH% && (
    @echo Error: Branch '%NEW_BRANCH%' already exists
    @popd
    @exit /b 1
)

@REM Create and checkout new branch
@git checkout -b %NEW_BRANCH% %BASE_BRANCH% >nul 2>&1 && (
    @echo Successfully created and switched to branch '%NEW_BRANCH%' from '%BASE_BRANCH%' in '%GIT_DIR%'
) || (
    @echo Error: Failed to create branch '%NEW_BRANCH%'
    @popd
    @exit /b 1
)

@popd
@endlocal