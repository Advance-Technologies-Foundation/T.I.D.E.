using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

	#region Class: AtfProcessUserTask_RunOSCommand

	/// <exclude/>
	public partial class AtfProcessUserTask_RunOSCommand
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			StringBuilder output = new StringBuilder();
			StringBuilder error = new StringBuilder();
			if(WorkingDirectory.IsNullOrEmpty()) {
				IsError = true;
				ErrorMessage = "Working directory is not set";
				return true;
			}
			if(!string.IsNullOrWhiteSpace(TransformerName)) {
				ITextTransformer transformer = ClassFactory.Get<ITextTransformer>(TransformerName);
				Arguments = transformer.Transform(Arguments);
			}
			try {
				ProcessStartInfo startInfo = new ProcessStartInfo {
					FileName = FileName,
					Arguments = Arguments,
					UseShellExecute = false,
					CreateNoWindow = true,
					WorkingDirectory = WorkingDirectory,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
				};
				using (System.Diagnostics.Process process = new System.Diagnostics.Process()) {
					process.StartInfo = startInfo;
					
					if(WaitForExit) {
						
						process.OutputDataReceived += (sender, e) => {
							if (!string.IsNullOrEmpty(e.Data)) {
								output.AppendLine(e.Data);
							}
						};
						process.ErrorDataReceived += (sender, e) => {
							if (!string.IsNullOrEmpty(e.Data)) {
								error.AppendLine(e.Data);
							}
						};
						process.Start();
						process.BeginOutputReadLine();
						process.BeginErrorReadLine();
						process.WaitForExit();
						ErrorMessage = error + "\n"+output;
						
					}else {
						process.Start();
					}
				}
			} catch (Exception ex) {
				SetError(ex.Message);
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

		private void SetError(string message) {
			ErrorMessage = message;
			IsError = true;
		}
	}

	#endregion

}

