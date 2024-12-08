using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	public interface IConsoleGit {

		#region Methods: Public

		ConsoleGitResult Execute(ConsoleGitArgs args);

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

		private static ConsoleGitResult StartProcess(ConsoleGitArgs args){
			try {
				
				ErrorOr<FileInfo> exePathOrError = HelperFunctions.GetConsoleGitPath();
				if(exePathOrError.IsError) {
					return new ConsoleGitResult(-1,
												$"Code:{exePathOrError.Errors.FirstOrDefault().Code} - Description: {exePathOrError.Errors.FirstOrDefault().Description}");
				}
				
				ProcessStartInfo startInfo = new ProcessStartInfo {
					FileName = exePathOrError.Value.FullName,
					Arguments = args.ToString(),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};

				using (Process process = new Process()) {
					process.StartInfo = startInfo;
					StringBuilder errorBuilder = new StringBuilder();
					DataReceivedEventHandler errorHandler = ProcessReceivedData(errorBuilder);
					process.ErrorDataReceived += errorHandler;
					
					StringBuilder outputBuilder = new StringBuilder();
					DataReceivedEventHandler outputHandler = ProcessReceivedData(outputBuilder);
					process.OutputDataReceived += outputHandler;
					
					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					
					// Process Timeout - Add a timeout to prevent hanging:
					if (Task.Run(() => process.WaitForExit(args.ProcessTimeoutMs)).ConfigureAwait(false).GetAwaiter().GetResult() == false) {
						process.Kill();
						return new ConsoleGitResult(-1,
													$"Process timeout after {args.ProcessTimeoutMs / 1_000} seconds");
					}
					//process.WaitForExit();
					process.ErrorDataReceived -= errorHandler;
					process.OutputDataReceived -= outputHandler;
					
					return process.ExitCode == 0 
								? new ConsoleGitResult(process.ExitCode, outputBuilder.ToString()) 
								: new ConsoleGitResult(process.ExitCode, errorBuilder.ToString());
					
				}
			} catch (Exception ex) {
				return new ConsoleGitResult(-1, ex.Message);
			}
		}
		
		/// <summary>
		/// Thread Safety - The StringBuilder operations aren't thread-safe, so we need to lock them
		/// </summary>
		/// <param name="sb">String builder</param>
		/// <returns></returns>
		private static DataReceivedEventHandler ProcessReceivedData(StringBuilder sb){
			object lockObj = new object();
			return (sender, e) => {
				if (e.Data != null) {
					lock (lockObj) {
						sb.AppendLine(e.Data);
					}
				}
			};
		}

		#endregion

		#region Methods: Public

		public ConsoleGitResult Execute(ConsoleGitArgs args){
			return StartProcess(args);
		}

		#endregion

	}

	public class ConsoleGitArgs {

		#region Properties: Public

		/// <summary>
		/// Git command to execute
		/// </summary>
		public Commands Command { get; set; }

		/// <summary>
		/// Repository Url
		/// </summary>
		public string GitUrl { get; set; }

		/// <summary>
		/// Repository Password or access token
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Repository Folder path
		/// </summary>
		public string RepoDir { get; set; }

		
		/// <summary>
		/// Repository User name
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Git commit message
		/// </summary>
		public string CommitMessage { get; set; }
		
		
		/// <summary>
		/// Maximum time to wait for the process to complete
		/// </summary>
		public int ProcessTimeoutMs { get; set; } = 60_000;

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
		AddAll,
		Push,
		Commit
	}
	
	public class ConsoleGitResult {

		public ConsoleGitResult(int exitCode, string output){
			
			ExitCode = exitCode;
			if(ExitCode == 0 ) {
				Output = output;
			}else {
				ErrorMessage = output;
			}
		}

		public int ExitCode { get; }
		
		public string ErrorMessage { get; }
		
		public string Output { get; }

	}
	
}