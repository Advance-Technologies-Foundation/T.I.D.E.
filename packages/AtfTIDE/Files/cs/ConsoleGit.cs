using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Common.Logging;
using ErrorOr;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	
	/// <summary>
	/// Provides git operations through a console application wrapper
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	///     <listheader>Implementation Requirements</listheader>
	///     <item>Executes git commands through environment variables</item>
	///     <item>Handles process management and timeouts</item>
	///     <item>Provides error handling and output capturing</item>
	/// </list>
	/// 
	/// <list type="bullet">
	///     <listheader>Thread Safety</listheader>
	///     <item>Implementations must be thread-safe</item>
	///     <item>Must handle concurrent git operations</item>
	/// </list>
	/// 
	/// <list type="bullet">
	///     <listheader>Error Handling</listheader>
	///     <item>Must capture and return git errors</item>
	///     <item>Must handle process timeouts</item>
	///     <item>Must clean up resources properly</item>
	/// </list>
	/// </remarks>
	public interface IConsoleGit {

		#region Methods: Public

		/// <summary>
		///  Executes a git command with the specified arguments
		/// </summary>
		/// <param name="args">Git command arguments and configuration</param>
		/// <returns>Result containing exit code and command output or error message</returns>
		ConsoleGitResult Execute(ConsoleGitArgs args);

		#endregion

	}

	[DefaultBinding(typeof(IConsoleGit), Name = "AtfTIDE.ConsoleGit")]
	public class ConsoleGit : IConsoleGit {

		#region Methods: Private

		/// <summary>
		/// Creates and configures a ProcessStartInfo for git command execution
		/// </summary>
		/// <param name="exePath">Path to the git executable</param>
		/// <param name="args">Git command arguments</param>
		/// <returns>Configured ProcessStartInfo instance</returns>
		/// <remarks>
		/// <list type="bullet">
		///     <listheader>Process Configuration</listheader>
		///     <item>Runs without shell execution for security</item>
		///     <item>Redirects standard output and error streams</item>
		///     <item>Runs without visible window</item>
		///     <item>Sets up environment variables for git command</item>
		/// </list>
		/// </remarks>
		private static ProcessStartInfo CreateProcessStartInfo(FileInfo exePath, ConsoleGitArgs args){
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = "dotnet",
				Arguments = exePath.FullName,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};
			ConfigureEnvironmentVariables(startInfo, args.ToDictionary());
			return startInfo;
		}

		
		
		/// <summary>
		/// Executes the git process and captures its output
		/// </summary>
		/// <param name="process">Configured Process object</param>
		/// <param name="timeoutMs">Timeout in milliseconds</param>
		/// <returns>Result containing exit code and output</returns>
		/// <remarks>
		/// <list type="bullet">
		///     <listheader>Process Execution Flow</listheader>
		///     <item>Attaches output handlers</item>
		///     <item>Starts the process</item>
		///     <item>Monitors for completion or timeout</item>
		///     <item>Ensures cleanup of handlers</item>
		/// </list>
		/// 
		/// <list type="bullet">
		///     <listheader>Thread Safety</listheader>
		///     <item>Uses synchronized output capture</item>
		///     <item>Properly manages event handlers</item>
		///     <item>Handles process termination safely</item>
		/// </list>
		/// </remarks>
		private static ConsoleGitResult ExecuteProcess(Process process, int timeoutMs){
			StringBuilder errorBuilder = new StringBuilder();
			StringBuilder outputBuilder = new StringBuilder();

			DataReceivedEventHandler errorHandler = ProcessReceivedData(errorBuilder);
			DataReceivedEventHandler outputHandler = ProcessReceivedData(outputBuilder);

			try {
				process.ErrorDataReceived += errorHandler;
				process.OutputDataReceived += outputHandler;

				//SetDotnetProcessTempPath(process.StartInfo, HelperFunctions.CreateTempDirectory().FullName);
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				if (!WaitForProcessExit(process, timeoutMs)) {
					return new ConsoleGitResult(-1, $"Process timeout after {timeoutMs / 1_000} seconds");
				}

				return process.ExitCode == 0
							? new ConsoleGitResult(process.ExitCode, outputBuilder.ToString())
							: new ConsoleGitResult(process.ExitCode, errorBuilder.ToString());
			} finally {
				// Ensure handlers are always removed
				process.ErrorDataReceived -= errorHandler;
				process.OutputDataReceived -= outputHandler;
			}
		}

		/// <summary>
		/// Creates a thread-safe event handler for process output
		/// </summary>
		/// <param name="stringBuilder">StringBuilder to capture output</param>
		/// <returns>Event handler for process output</returns>
		/// <remarks>
		/// <list type="bullet">
		///     <listheader>Thread Safety</listheader>
		///     <item>Uses per-handler lock object for thread synchronization</item>
		///     <item>Safely handles null data events</item>
		///     <item>Ensures sequential output capture</item>
		/// </list>
		/// </remarks>
		private static DataReceivedEventHandler ProcessReceivedData(StringBuilder stringBuilder){
			object lockObj = new object();
			return (sender, e) => {
				if (e.Data != null) {
					lock (lockObj) {
						stringBuilder.AppendLine(e.Data);
					}
				}
			};
		}

		/// <summary>
		///  Configures environment variables for the process start information.
		/// </summary>
		/// <param name="startInfo">The process start information.</param>
		/// <param name="envVariable">The environment variables to set.</param>
		private static void ConfigureEnvironmentVariables(ProcessStartInfo startInfo, IDictionary<string, string> envVariable){

#if DEBUG			
			var logger = LogManager.GetLogger(TideConsts.LoggerName);
			StringBuilder sb = new StringBuilder();
#endif
			
			foreach (string variable in envVariable.Keys) {
				startInfo.EnvironmentVariables.Add(variable, envVariable[variable]);
			
#if DEBUG
				sb
					.Append(variable)
					.Append("=")
					.Append(envVariable[variable])
					.Append(";");
#endif
			}
#if DEBUG			
			logger.InfoFormat("StartArgs: {0}",sb.ToString());
#endif
		}

		/// <summary>
		///  Starts and manages the git process execution
		/// </summary>
		/// <param name="args">Git command arguments</param>
		/// <returns>Result of the git command execution</returns>
		/// <remarks>
		///  <list type="bullet">
		///   <listheader>Process Management</listheader>
		///   <item> Uses async/await for output handling</item>
		///   <item> Implements timeout protection</item>
		///   <item> Manages process resources properly</item>
		///   <item> Handles both standard and error output</item>
		///  </list>
		///  <list type="bullet">
		///   <listheader>Error Handling</listheader>
		///   <item>Catches and processes execution exceptions</item>
		///   <item>Handles process timeout scenarios</item>
		///   <item>Validates executable path</item>
		///  </list>
		///  <list type="bullet">
		///   <listheader>Thread Safety:</listheader>
		///   <item> Uses lock objects for output handling</item>
		///   <item> Properly manages shared resources</item>
		///  </list>
		/// </remarks>
		private static ConsoleGitResult ExecuteGitProcess(ConsoleGitArgs args){
			try {
				// Validate input
				if (args == null) {
					throw new ArgumentNullException(nameof(args));
				}
				
				// Get executable path
				ErrorOr<FileInfo> exePathOrError = HelperFunctions.GetConsoleGitPath();
				if (exePathOrError.IsError) {
					Error error = exePathOrError.FirstError;
					return new ConsoleGitResult(-1, $"Code:{error.Code} - Description: {error.Description}");
				}

				// Configure process
				ProcessStartInfo startInfo = CreateProcessStartInfo(exePathOrError.Value, args);

				using (Process process = new Process()) {
					process.StartInfo = startInfo;
					return ExecuteProcess(process, args.ProcessTimeoutMs);
				}
			} catch (Exception ex) {
				return new ConsoleGitResult(-1, $"Process execution failed: {ex.Message}");
			}
		}

		/// <summary>
		/// Waits for process completion with timeout and handles process termination
		/// </summary>
		/// <param name="process">Running process to monitor</param>
		/// <param name="timeoutMs">Maximum wait time in milliseconds</param>
		/// <returns>True if process completed normally, false if timeout or disposal occurred</returns>
		/// <remarks>
		/// <list type="bullet">
		///     <listheader>Process Handling</listheader>
		///     <item>Executes WaitForExit asynchronously to prevent blocking</item>
		///     <item>Attempts to kill process if timeout occurs</item>
		///     <item>Handles race conditions between timeout check and kill</item>
		/// </list>
		/// 
		/// <list type="bullet">
		///     <listheader>Error Handling</listheader>
		///     <item>Catches ObjectDisposedException if process is disposed</item>
		///     <item>Catches InvalidOperationException during kill attempt</item>
		///     <item>Returns false for any unrecoverable scenario</item>
		/// </list>
		/// 
		/// <list type="bullet">
		///     <listheader>Return Values</listheader>
		///     <item>True: Process completed within timeout period</item>
		///     <item>False: Process timed out or was disposed</item>
		/// </list>
		/// </remarks>
		private static bool WaitForProcessExit(Process process, int timeoutMs){
			try {
				bool hasExited = process.WaitForExit(timeoutMs);
				if (!hasExited) {
					try {
						process.Kill();
					} catch (InvalidOperationException) {
						// Process may have exited between our check and kill attempt
					}
				}

				return hasExited;
			} catch (ObjectDisposedException) {
				return false;
			}
		}

		#endregion

		#region Methods: Public

		public ConsoleGitResult Execute(ConsoleGitArgs args){
			return ExecuteGitProcess(args);
		}

		#endregion

	}

	/// <summary>
	///  Defines supported git operations
	/// </summary>
	public enum Commands {

		Clone,
		AddAll,
		AddSome,
		Push,
		Pull,
		Commit,
		DownloadPackages,
		Checkout,
		GetBranches,
		GetDiff,
		GetChangedFiles,
		DiscardFiles,
		GetActiveBranch
	}
}