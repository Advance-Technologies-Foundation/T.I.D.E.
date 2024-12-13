namespace AtfTIDE {
	/// <summary>
	/// Represents the result of a git command execution
	/// </summary>
	/// <remarks>
	/// Provides both success and error information:
	/// <list type="bullet">
	/// <item> ExitCode: 0 for success, non-zero for errors</item>
	/// <item> Output: Contains command output for successful operations</item>
	/// <item>ErrorMessage: Contains error details for failed operations</item>
	/// </list>
	/// </remarks>
	public class ConsoleGitResult {

		
		/// <summary>
		/// Creates a new instance of ConsoleGitResult
		/// </summary>
		/// <param name="exitCode">Process exit code (0 for success)</param>
		/// <param name="output">Process output or error message</param>
		public ConsoleGitResult(int exitCode, string output){
			ExitCode = exitCode;
			if(ExitCode == 0 ) {
				Output = output;
			}else {
				ErrorMessage = output;
			}
		}

		/// <summary>
		/// Exit code of ConsoleGit process
		/// </summary>
		public int ExitCode { get; }
		
		/// <summary>
		/// Console output captured from the process's Error output.
		/// </summary>
		public string ErrorMessage { get; }
		
		/// <summary>
		/// Console output captured from the process's standard output.
		/// </summary>
		public string Output { get; }

	}
}