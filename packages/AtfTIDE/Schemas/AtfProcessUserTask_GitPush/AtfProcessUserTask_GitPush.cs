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

	#region Class: AtfProcessUserTask_GitPush

	/// <exclude/>
	public partial class AtfProcessUserTask_GitPush
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			string repositoryName = HelperFunctions.FetchRepositoryName(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Push,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryName).ToString(),
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

