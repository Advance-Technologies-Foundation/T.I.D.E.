using AtfTIDE;
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

	#region Class: AtfProcessUserTask_CloneRepository

	/// <include />
	public partial class AtfProcessUserTask_CloneRepository {
	
		/// <summary>
		/// Hey, this is what's running when the process is executed.
		/// </summary>
		/// <param name="context">Creatio will pass context to this method</param>
		/// <returns>Always <c>true</c></returns>
		/// <remarks>
		/// <see href="../UserTask/AtfProcessUserTask_CloneRepository.html">See Wiki</see>
		/// </remarks>
		protected override bool InternalExecute(ProcessExecutingContext context){
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Clone,
				GitUrl = repositoryInfo.GitUrl,
				Password = repositoryInfo.Password,
				UserName = repositoryInfo.UserName,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString(),
			};
			
			ConsoleGitResult gitCommandResult = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);
			
			if(gitCommandResult.ExitCode != 0) {
				IsError = true;
				ErrorMessage = gitCommandResult.ErrorMessage;
			}else {
				RepositoryFolderPath = args.RepoDir;
				Output = gitCommandResult.Output;
			}
			return true;
		}
	}

	
	
	
	#endregion

}

