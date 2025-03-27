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

	#region Class: AtfProcessUserTask_GitCommit

	public partial class AtfProcessUserTask_GitCommit
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			RepositoryInfo repoInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Commit,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repoInfo.Name).ToString(),
				CommitMessage = CommitMessage,
				CommitAuthorName = string.IsNullOrWhiteSpace(repoInfo.UserName) ? UserConnection.CurrentUser.Name : repoInfo.UserName,
				CommitAuthorEmail = repoInfo.UserName
			};
			ConsoleGitResult gitCommandResult = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);
			
			if(gitCommandResult.ExitCode != 0) {
				IsError = true;
				ErrorMessage = gitCommandResult.ErrorMessage;
			}else {
				Output = gitCommandResult.Output;
			}
			return true;
		}

		#endregion

		#region Methods: Public

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion

}

