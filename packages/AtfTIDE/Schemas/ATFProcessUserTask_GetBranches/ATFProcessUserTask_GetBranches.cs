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

	#region Class: ATFProcessUserTask_GetBranches

	/// <exclude/>
	public partial class ATFProcessUserTask_GetBranches
	{

		#region Methods: Private

		private CompositeObject CreateCompositeObject(string branchName) {
			CompositeObject compositeObject = new CompositeObject();
			compositeObject.Add("Name", branchName);
			return compositeObject;
		}

		#endregion


		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			RepositoryInfo repoInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = Commands.GetBranches,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repoInfo.Name).ToString()
			};
			ConsoleGitResult gitCommandResult = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);

			if (gitCommandResult.ExitCode != 0) {
				IsError = true;
				ErrorMessage = gitCommandResult.ErrorMessage;
				return true;
			}
			var branches = new CompositeObjectList<CompositeObject>();
			Common.Json.Json.Deserialize<BranchesCommandResponse>(gitCommandResult.Output).branches.ToList()
				.ForEach(branch => branches.Add(CreateCompositeObject(branch.name)));
			Branches = branches;
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

	public class BranchesCommandResponse
	{
		public Branch[] branches
		{
			get; set;
		}
	}

	public class Branch
	{
		public string name
		{
			get; set;
		}
	}


}

