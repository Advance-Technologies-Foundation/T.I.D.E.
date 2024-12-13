using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AtfTIDE {
	/// <summary>
	/// Represents configuration and arguments for git operations
	/// </summary>
	/// <remarks>
	/// Environment Variables Generated:
	/// - Command: Git command to execute
	/// - GitUrl: Repository URL
	/// - UserName: Git username
	/// - Password: Git password/token
	/// - RepoDir: Repository directory
	/// - CommitMessage: Commit message
	/// - ProcessTimeoutMs: Process timeout in milliseconds
	/// </remarks>
	public class ConsoleGitArgs {

		#region Properties: Public

		/// <summary>
		/// Git command to execute
		/// </summary>
		/// <remarks>
		/// Supported commands are defined in the Commands enum
		/// </remarks>
		public Commands Command { get; set; }

		/// <summary>
		/// Repository URL (HTTPS or SSH)
		/// </summary>
		public string GitUrl { get; set; }

		/// <summary>
		/// Repository password or access token
		/// </summary>
		/// <remarks>
		/// For security, prefer using access tokens over passwords
		/// Token should have minimal required permissions
		/// </remarks>
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
		
		/// <summary>
		/// Name of the author to be used in the commit
		/// </summary>
		/// <remarks>
		/// Required for Commit operations.
		/// Should match the git config user.name format
		/// </remarks>
		public string CommitAuthorName { get; set; }
		
		/// <summary>
		/// Email of the author to be used in the commit
		/// </summary>
		/// <remarks>
		/// Required for Commit operations.
		/// Should match the git config user.email format
		/// Must be a valid email address
		/// </remarks>
		public string CommitAuthorEmail { get; set; }
		
		
		public Uri CreatioUrl { get; set; }
		public string BPMLOADER { get; set; }
		public string ASPXAUTH { get; set; }
		public string BPMCSRF { get; set; }
		public string UserType { get; set; }
		public string BPMSESSIONID { get; set; }
		
		
		#endregion

		#region Methods: Public
		
		/// <summary>
		/// Converts all public properties to environment variables dictionary
		/// </summary>
		/// <returns>Dictionary of environment variables</returns>
		/// <remarks>
		/// <list type="bullet">
		/// <listheader>Conversion Rules:</listheader>
		/// <item>Skips null values</item>
		/// <item>Formats dates as ISO 8601</item>
		/// <item>Uses invariant culture for formatting numbers</item>
		/// <item>Empty strings are included (unlike null values)</item>
		/// </list>
		/// </remarks>
		public Dictionary<string, string> ToDictionary() {
			Dictionary<string, string> dict = new Dictionary<string, string>();
			IEnumerable<PropertyInfo> properties = this.GetType().GetProperties()
				.Where(p => p.CanRead);  // Only get properties that can be read
			foreach (PropertyInfo prop in properties) {
				object value = prop.GetValue(this);
				string stringValue;
				switch (value) {
					case null:
						continue;
					case DateTime dt:
						stringValue = dt.ToString("O");  // ISO 8601
						break;
					case IFormattable formattable:
						stringValue = formattable.ToString(null, CultureInfo.InvariantCulture);
						break;
					default:
						stringValue = value.ToString();
						break;
				}
				dict.Add($"TIDE_{prop.Name}", stringValue);
			}
			return dict;
		}
		
		#endregion

	}
}