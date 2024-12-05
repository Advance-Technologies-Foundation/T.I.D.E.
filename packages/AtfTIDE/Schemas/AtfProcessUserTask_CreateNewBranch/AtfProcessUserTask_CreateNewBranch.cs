using System.IO;
using System.Linq;
using AtfTIDE;
using ErrorOr;
using Terrasoft.Core.Factories;

namespace Terrasoft.Core.Process.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;

	#region Class: AtfProcessUserTask_CreateNewBranch

	/// <exclude/>
	public partial class AtfProcessUserTask_CreateNewBranch
	{

		protected override bool InternalExecute(ProcessExecutingContext context){
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string repositoryDirectory = Path.Combine(baseDir, "conf","tide", RepositoryName);
			
			
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Clone,
				GitUrl = RepositoryUrl,
				Password = AccessToken,
				UserName = UserName,
				RepoDir = repositoryDirectory,
			};
			
			ErrorOr<Success> result = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);
			
			result.Match<Action>(
				success => ()=>{
					ErrorMessage = string.Empty;
					IsError = false;
				},
				error => ()=> {
					ErrorMessage =$"{error.FirstOrDefault().Code} {error.FirstOrDefault().Description}";
					IsError = true;
				}
			).Invoke();
			return true;
			
			// var baseDir = AppDomain.CurrentDomain.BaseDirectory;
			// var pkgFolderPath = Path.Combine(baseDir, "Terrasoft.Configuration", "Pkg", "AtfTIDE","Files");
			// var clioFolderPath = Path.Combine(baseDir, "Terrasoft.Configuration", "Pkg", "AtfTIDE","Files","clio");
			// var confFolderPath = Path.Combine(baseDir, "conf");
			// var tideFolderPath = Path.Combine(confFolderPath,"tide");
			//
			// if(!Directory.Exists(tideFolderPath)) {
			// 	Directory.CreateDirectory(tideFolderPath);
			// }
			// string repositoryFolderPath = Path.Combine(tideFolderPath, RepositoryName);
			// if(!Directory.Exists(repositoryFolderPath)) {
			// 	Directory.CreateDirectory(repositoryFolderPath);
			// }
			//
			// string branchFolderPath = Path.Combine(repositoryFolderPath, BranchName);
			// if(!Directory.Exists(branchFolderPath)) {
			// 	Directory.CreateDirectory(branchFolderPath);
			// }
			
			// Clone
			// new branch
			
			
			
			
			
			
			ErrorMessage = string.Empty;
			IsError = false;
			return true;
		}
		
		

	}

	#endregion

}

