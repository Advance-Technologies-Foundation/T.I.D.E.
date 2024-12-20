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

	#region Class: AtfProcessUserTask_CheckoutBranch

	/// <exclude/>
	public partial class AtfProcessUserTask_CheckoutBranch
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			
			
			Entity entity = UserConnection.EntitySchemaManager
			                              .FindInstanceByName("AtfRepositoryBranch")
			                              .CreateEntity(UserConnection);
			
			entity.FetchFromDB(RepositoryBranch);
			Guid repositoryId = entity.GetTypedColumnValue<Guid>("AtfRepositoryId");
			
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(repositoryId, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Checkout,
				GitUrl = repositoryInfo.GitUrl,
				BranchName = entity.GetTypedColumnValue<string>("AtfName"),
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

