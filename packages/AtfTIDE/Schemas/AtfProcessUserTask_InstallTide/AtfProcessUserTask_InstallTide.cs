using System.Diagnostics;
using System.IO;
using AtfTIDE;
using AtfTIDE.Logging;
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

	#region Class: AtfProcessUserTask_InstallTide

	/// <exclude/>
	public partial class AtfProcessUserTask_InstallTide
	{

		#region Methods: Protected

		private static void SetDotnetProcessTempPath(ProcessStartInfo processStartInfo, string tempPath) {
			processStartInfo.EnvironmentVariables["TMP"] = tempPath;
			processStartInfo.EnvironmentVariables["TEMP"] = tempPath;
			string profilePath = Path.Combine(tempPath, "dotnet");
			processStartInfo.EnvironmentVariables["DOTNET_CLI_HOME"] = profilePath;
			processStartInfo.EnvironmentVariables["USERPROFILE"] = profilePath;
			processStartInfo.EnvironmentVariables["APPDATA"] = Path.Combine(profilePath, "AppData", "Roaming");
			processStartInfo.EnvironmentVariables["LOCALAPPDATA"] = Path.Combine(profilePath, "AppData", "Local");
		}
		protected override bool InternalExecute(ProcessExecutingContext context) {
			
			var logger = TideApp.Instance.GetRequiredService<ILiveLogger>();
			logger.LogInfo("Starting TIDE installation...");
			
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			ITextTransformer transformer = ClassFactory.Get<ITextTransformer>("UrlAppender");
			var clioPath = SysSettings.GetValue(UserConnection,"AtfClioFilePath").ToString();
			
			string userName = SysSettings.GetValue(UserConnection,"AtfUserNameForClio", "Supervisor");
			string password = SysSettings.GetValue(UserConnection,"AtfPasswordForClio", "Supervisor");
			
			string arguments = $"{clioPath} tide -l {userName} -p {password}";
			string transformedArguments = transformer.Transform(arguments);
			
			logger.LogInfo($"Starting dotnet {transformedArguments}");
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = "dotnet",
				Arguments = transformedArguments,
				UseShellExecute = false,
				CreateNoWindow = true,
				//UseShellExecute = true,
				//CreateNoWindow = false,
				WorkingDirectory = clioDir.FullName,
			};
			SetDotnetProcessTempPath(startInfo, HelperFunctions.CreateTempDirectory().FullName);
			using (System.Diagnostics.Process process = new System.Diagnostics.Process()) {
				process.StartInfo = startInfo;
				process.Start();
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

