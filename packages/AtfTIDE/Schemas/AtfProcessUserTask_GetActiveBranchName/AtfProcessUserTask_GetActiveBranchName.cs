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
	using System.Linq;
	using AtfTIDE;
	using ErrorOr;
	using Terrasoft.Core.Factories;

	#region Class: AtfProcessUserTask_GetActiveBranchName

	/// <exclude/>
	public partial class AtfProcessUserTask_GetActiveBranchName
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			RepositoryInfo repoInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = Commands.GetActiveBranch,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repoInfo.Name).ToString(),
				Silent = true
			};
			ConsoleGitResult gitCommandResult = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);

			if (gitCommandResult.ExitCode != 0) {
				IsError = true;
				ErrorMessage = gitCommandResult.ErrorMessage;
				return true;
			}
			ActiveBranchName = Common.Json.Json.Deserialize<ActiveBranchResponse>(gitCommandResult.Output).name;
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

	#region Class: Public
	
	public class ActiveBranchResponse
	{
		public string name
		{
			get; set;
		}
	}
	
	#endregion
}

