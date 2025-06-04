using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Terrasoft.Core;
using Terrasoft.Core.Configuration;
using Terrasoft.Core.Factories;

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
		public int ProcessTimeoutMs { get; set; } = 600_000;
		
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
		public Uri CreatioUrl => GetCreatioUrl();
		// public string BPMLOADER { get; set; }
		// public string ASPXAUTH { get; set; }
		// public string BPMCSRF { get; set; }
		// public string UserType { get; set; }
		// public string BPMSESSIONID { get; set; }

		public bool Silent { get; set; }

		public string BranchName { get; set; }

		public string Files { get; set; }
		
		public string CreatioUserName => GetCreatioUserName();
		public string CreatioPassword => GetCreatioPassword();
		public bool IsFramework => IsNetFramework();
		
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

		
		private static Uri GetCreatioUrl() {
			
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Dictionary<string, string> webSettings = HelperFunctions.ClioArguments[userConnection.CurrentUser.Id];
			string url = webSettings["SystemUrl"]; //http:localhost:8080/0 ??http:localhost:8080 
			bool isFramework = url.EndsWith("/0");
			if(isFramework) {
				url = url.Substring(0,url.Length - 2);
			}
			return new Uri(url);
		}
		
		private static string GetCreatioUserName() {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			string userName = SysSettings
				.GetValue(userConnection, "AtfUserNameForClio", "Supervisor");
			return userName;
			
		}
		
		private static string GetCreatioPassword() {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			string userName = SysSettings
				.GetValue(userConnection, "AtfPasswordForClio", "Supervisor");
			return userName;
		}
		
		private static bool IsNetFramework() {
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Dictionary<string, string> webSettings = HelperFunctions.ClioArguments[userConnection.CurrentUser.Id];
			string url = webSettings["SystemUrl"]; //http:localhost:8080/0 ??http:localhost:8080 
			return url.EndsWith("/0");
		}
		
	}
}