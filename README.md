<div style="display: flex; align-items: center; justify-content: start; background: #1E1E1E; color: #FFFFFF; padding: 20px; font-family: Arial, sans-serif; border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.2);">
  <!-- Logo on the left -->
  <a href="/icons/tide.svg" target="_blank" style="flex-shrink: 0; margin-right: 20px; text-decoration: none;">
    <img src="/icons/tide.svg" alt="T.I.D.E. Logo" style="width: 128px; height: auto; border-radius: 8px; display: block;" />
  </a>

  <!-- Title and description on the right -->
  <div style="flex: 1; text-align: left;">
    <h1 style="font-size: 2.5em; font-weight: bold; margin: 0; color: #000000ff;">T.I.D.E. </h1>
    <p style="font-size: 1.2em; margin-top: 10px; margin-bottom: 0; color: #000000ff;">Team Integration Developer Environment for Creatio Developers</p>
  </div>
</div>


🚀 About T.I.D.E.
T.I.D.E. is a powerful application designed to bridge the gap for NOCODE developers working in Creatio. It empowers teams to seamlessly integrate with GIT directly from the Creatio environment, eliminating the need for additional external tools.

🌟 Key Features
Git Integration Made Easy: Simplify version control for NOCODE developers within Creatio.
Streamlined Workflow: Perform essential Git operations like commits, branches, and merges without leaving the Creatio interface.
No Additional Tools Required: Focus on development without juggling multiple platforms.
Team Collaboration: Enhance team productivity with integrated version control tailored for NOCODE workflows.

🎯 Who Is It For?
T.I.D.E. is built specifically for:
NOCODE Developers: Simplifying Git usage in Creatio without technical complexity.
Creatio Development Teams: Streamlining integration and collaboration workflows.
Organizations: Optimizing development efficiency by reducing the need for external Git tools.

💡 Why Choose T.I.D.E.?
Seamless integration into the Creatio ecosystem.
Designed to meet the needs of NOCODE developers.
Eliminates the learning curve of traditional Git tools.
Boosts team productivity and collaboration.
🛠️ How to Use
Install T.I.D.E. into your Creatio environment.
Configure Git settings directly in the application.
Start managing your project with version control tools designed for simplicity.

🌊 Dive Into T.I.D.E.
Ready to revolutionize your Creatio development workflow?
T.I.D.E. is your one-stop solution for effortless Git integration and team collaboration.

Make waves in development with T.I.D.E. 🌊!

# Installing TIDE

1. Go to [TIDE Releases](https://github.com/Advance-Technologies-Foundation/T.I.D.E./releases).
2. Download application package. 
3. Install to Creatio via Application Hub: New application -> Install from file -> Select file.

# Overview
TIDE provides you a user interface for synchronizing changes to Git repositories. It'll use clio and Git to synchronize the changes between Creatio instance and source control repository. 

To authenticate Git operations on your behalf, you'll need to generate a personal access token for your repository. See [creating access token](/access-tokens.md). 

TIDE allows you to work with multiple Git repositories. You do all source control operations within a context of a repository. You can see the list of repositories in the *TIDE* section.

# Connecting to a Repository
1. Press *Add Repository* button, go to *General information* tab.
2. *User name* - your user email.
3. *Access Тoken* - paste your personal access token.
4. *Repository Url* - paste repository clone URL. 
5. Press *Save* 

# Installing Application from Git
1. Set your active [branch](#branching)
2. Press *Update from Git* to download the application packages from repository and install the application to Creatio.
2. Now you can start making changes using Creatio no-code tools.

# Synchronizing Changes
Go to *Source Control* tab.
1. Press *Commit to Git* to commit changes to Git
2. Enter commit message

# Reviewing Changes
Go to *Source Control* tab.
1. Press Load changes from Creatio.
2. List of changed files will be populated.
3. Now review all changed files.

# Branching
> **_NOTE:_**  New branches have to be created outside TIDE.
1. Go to *Branches* tab
2. Press *Synchronize branches with repository* to update the list of available branches.
3. Press *Set active branch* to set current working branch. Active branch will be displayed on the left side on the repository page.

# Troubleshooting
You can access log by clicking *View logs* button at the top of the toolbar. The logs will be shown in the right-hand side panel.

## Possible Issues
During installation, TIDE installs to the target Creatio two additional components:
1. Git console. 
2. clio-gate.

If your source control operations are failing, check that those two components are installed. TIDE -> Actions -> Install console git

Also check if your token is valid and has read/write permissions.

## Business Processes 
Open Process log, see what business processes from package *AtfTIDE* were executed. Select a process -> *Execution diagram* to see its execution details. You can [enable business process tracing](https://academy.creatio.com/docs/user/bpm_tools/business_process_administration/trace_process_parameters/trace_process) to get more details.