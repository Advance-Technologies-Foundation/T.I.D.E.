using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using ErrorOr;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	public interface IConsoleGit {

		#region Methods: Public

		ErrorOr<Success> Execute(ConsoleGitArgs args);

		#endregion

	}

	[DefaultBinding(typeof(IConsoleGit), Name = "AtfTIDE.ConsoleGit")]
	public class ConsoleGit : IConsoleGit {

		#region Methods: Private

		// args will have the following shape
		// args[0] - command
		// args[1] - gitUrl
		// args[2] - userName
		// args[3] - password
		// args[4] - repoDir

		private ErrorOr<Success> StartProcess(ConsoleGitArgs args){
			try {
				string baseDir = AppDomain.CurrentDomain.BaseDirectory;
				string execFolderPath = Path.Combine(baseDir, "Terrasoft.Configuration", "Pkg", "AtfTIDE", "Files",
													"exec", "ConsoleGit.exe");

				// Validate executable path exists
				if (!File.Exists(execFolderPath)) {
					return Error.Failure("Process error", "Executable not found");
				}
				
				ProcessStartInfo startInfo = new ProcessStartInfo {
					FileName = execFolderPath,
					Arguments = args.ToString(),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};

				using (Process process = new Process()) {
					process.StartInfo = startInfo;
					StringBuilder errorBuilder = new StringBuilder();
					process.ErrorDataReceived += (sender, e) => {
						if (e.Data != null) {
							errorBuilder.AppendLine(e.Data);
						}
					};
					process.Start();
					process.BeginErrorReadLine();
					process.WaitForExit();
					if (process.ExitCode == 0) {
						return Result.Success;
					}
					string error = errorBuilder.ToString();
					return Error.Failure(
						$"ProcessError {args.Command.ToString().ToLowerInvariant()} failed with error code {process.ExitCode}",
						$"Process failed with error code {process.ExitCode}\t{error}");
				}
			} catch (Exception ex) {
				return Error.Failure("Process error", ex.Message);
			}
		}

		#endregion

		#region Methods: Public

		public ErrorOr<Success> Execute(ConsoleGitArgs args){
			return StartProcess(args);
		}

		#endregion

	}

	public class ConsoleGitArgs {

		#region Properties: Public

		public Commands Command { get; set; }

		public string GitUrl { get; set; }

		public string Password { get; set; }

		public string RepoDir { get; set; }

		public string UserName { get; set; }

		#endregion

		#region Methods: Public

		public override string ToString(){
			return
				$"{Command.ToString().ToLower(CultureInfo.InvariantCulture)} {GitUrl} {UserName} {Password} {RepoDir}";
		}

		#endregion

	}

	public enum Commands {

		Clone,
		Pull,
		Checkout

	}
}