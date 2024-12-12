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

	#region Class: AtfProcessUserTask_DownloadPackages

	public partial class AtfProcessUserTask_DownloadPackages
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			
			var webSettings = HelperFunctions.ClioArguments[UserConnection.CurrentUser.Id];
			
			var repoInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.DownloadPackages,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repoInfo.Name).ToString(),
				CreatioUrl = new Uri(webSettings["SystemUrl"]),
				BPMLOADER = webSettings["BPMLOADER"],
				BPMCSRF = webSettings["BPMCSRF"],
				UserType = webSettings["UserType"],
				BPMSESSIONID = webSettings["BPMSESSIONID"],
				ASPXAUTH = webSettings[".ASPXAUTH"],
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

