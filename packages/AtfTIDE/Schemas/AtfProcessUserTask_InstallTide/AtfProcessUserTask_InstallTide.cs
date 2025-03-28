using System.Diagnostics;
using System.IO;
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

	#region Class: AtfProcessUserTask_InstallTide

	/// <exclude/>
	public partial class AtfProcessUserTask_InstallTide
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			ITextTransformer transformer = ClassFactory.Get<ITextTransformer>("UrlAppender");
			var clioPath = SysSettings.GetValue(UserConnection,"AtfClioFilePath").ToString();
			
			string userName = SysSettings.GetValue(UserConnection,"AtfUserNameForClio", "Supervisor");
			string password = SysSettings.GetValue(UserConnection,"AtfPasswordForClio", "Supervisor");
			
			string arguments = $"{clioPath} tide -l {userName} -p {password}";
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = "dotnet",
				Arguments = transformer.Transform(arguments),
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = clioDir.FullName,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
			};
			
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

