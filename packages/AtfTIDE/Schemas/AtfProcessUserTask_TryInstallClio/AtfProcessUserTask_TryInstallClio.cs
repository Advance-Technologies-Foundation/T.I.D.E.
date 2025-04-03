using System.Diagnostics;
using System.IO;
using System.Linq;
using AtfTIDE;
using AtfTIDE.ClioInstaller;
using ErrorOr;
using Terrasoft.Core.Factories;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection;

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

	#region Class: AtfProcessUserTask_TryInstallClio

	/// <exclude/>
	public partial class AtfProcessUserTask_TryInstallClio
	{
		private const string ClioNugetPackageName = "creatio.clio";

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			// Clio is not installed or directory doesn't exist, proceed with installation
			if (!clioDir.Exists) {
				clioDir.Create();
			} else {
				clioDir.Delete(true);
				clioDir.Create();
			}

			IErrorOr<Success> result = TideApp.Create().InstallerApp.InstallClio();
			if(result.IsError) {
				ErrorMessage = $"{result.Errors.FirstOrDefault().Code} - {result.Errors.FirstOrDefault().Description}";
				IsError = true;

			} else {
				FileInfo[] clioFilePath = clioDir.GetFiles("clio.dll", SearchOption.AllDirectories);
				SysSettings.SetValue(UserConnection, "AtfClioFilePath", clioFilePath.First().FullName);
				SysSettings.SetValue(UserConnection, "AtfClioUpdateAvailable", false);
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

