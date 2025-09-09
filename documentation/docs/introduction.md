# Introduction

## Tech Stack
Tide is a Creatio Application wrapped in a clio workspace. The main artifact is AtfTIDE package.

Tide enables execution of git commands from Creatio Interface.
Solution consists of:
- Creatio Application
- Console Git


AtfTIDE package contains `AtfTIDE/Files/exec/archive.zip` file, which is an archive of `Console Git` release build.  

When AtfTIDE is installed:
- [ ] Archived is unzipped and copied to `/conf/consolegit` folder of Creatio (see: [`/WebServices/Tide/InstallConsoleGit`][InstallConsoleGit])
- [x] Clio installed from Nuget on first webapp restart (see [`AppEventListener`][AppEventListener])

[InstallConsoleGit](~/api/AtfTIDE.WebServices.Tide.html#AtfTIDE_WebServices_Tide_InstallConsoleGit)

## Creatio Application



## Console Git

ConsoleGit is a .NET8 (self-contained) console application that acts as a bridge between the Creatio interface and the underlying Git functionality. 
It receives commands from Creatio, executes corresponding Git operations using the [LibGit2Sharp] library, 
and returns results in a structured format. 
The project handles various Git actions such as cloning, committing, pushing, pulling, and status checks, 
enabling seamless integration of Git workflows within the Creatio environment.

[//]: # (named links)
[clio]: https://https://github.com/Advance-Technologies-Foundation/clio
[LibGit2Sharp]: https://github.com/libgit2/libgit2sharp
[InstallConsoleGit]: ~/api/AtfTIDE.WebServices.Tide.html#AtfTIDE_WebServices_Tide_InstallConsoleGit
[AppEventListener]: ~/api/AtfTIDE.AppEventListener.html#AtfTIDE_AppEventListener_OnAppStart_Terrasoft_Web_Common_AppEventContext_