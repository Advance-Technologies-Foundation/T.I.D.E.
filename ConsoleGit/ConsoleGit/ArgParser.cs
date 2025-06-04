using System.Diagnostics;
using System.Net;

namespace ConsoleGit;

public record CommandLineArgs {

	#region Constructors: Public

	public CommandLineArgs(){
		Command = Environment.GetEnvironmentVariable($"TIDE_{nameof(Command)}") ?? "";
		Uri.TryCreate(Environment.GetEnvironmentVariable($"TIDE_{nameof(GitUrl)}"), UriKind.Absolute, out Uri? gitUrl);
		GitUrl = gitUrl;
		UserName = Environment.GetEnvironmentVariable($"TIDE_{nameof(UserName)}") ?? "";
		Password = Environment.GetEnvironmentVariable($"TIDE_{nameof(Password)}") ?? "";
		RepoDir = Environment.GetEnvironmentVariable($"TIDE_{nameof(RepoDir)}") ?? "";
		CommitMessage = Environment.GetEnvironmentVariable($"TIDE_{nameof(CommitMessage)}") ?? "";
		CommitAuthorName = Environment.GetEnvironmentVariable($"TIDE_{nameof(CommitAuthorName)}") ?? "";
		CommitAuthorEmail = Environment.GetEnvironmentVariable($"TIDE_{nameof(CommitAuthorEmail)}") ?? "";
		Uri.TryCreate(Environment.GetEnvironmentVariable($"TIDE_{nameof(CreatioUrl)}"), UriKind.Absolute,
			out Uri? creatioUrl);
		CreatioUrl = creatioUrl;
		IsFramework = CreatioUrl?.LocalPath.StartsWith("/0", StringComparison.InvariantCulture) ?? true;
		BranchName = Environment.GetEnvironmentVariable($"TIDE_{nameof(BranchName)}") ?? "";
		Files = Environment.GetEnvironmentVariable($"TIDE_{nameof(Files)}") ?? "";
		
		bool.TryParse(Environment.GetEnvironmentVariable($"TIDE_{nameof(Silent)}") ?? "", out bool silent);
		
		CreatioPassword = Environment.GetEnvironmentVariable($"TIDE_{nameof(CreatioPassword)}") ?? "";
		CreatioUserName = Environment.GetEnvironmentVariable($"TIDE_{nameof(CreatioUserName)}") ?? "";
		
		Silent = silent;
	}


	#endregion

	#region Properties: Public
	public bool Silent { get; init; }
	
	public string Command { get; init; }

	public string CommitAuthorEmail { get; set; }

	public string CommitAuthorName { get; set; }

	public string CommitMessage { get; set; }

	public Uri? CreatioUrl { get; init; }
	public string CreatioUserName { get; init; }
	public string CreatioPassword { get; init; }

	public Uri? GitUrl { get; init; }

	public bool IsFramework { get; init; }

	public bool PackageName { get; init; }

	public string Password { get; init; }

	public string RepoDir { get; init; }

	public string UserName { get; init; }

	public string? UserType { get; init; }

	public string BranchName { get; set; }
	
	public string Files { get; set; }
	
	

	#endregion

	#region Methods: Public

	/// <summary>
	///  Creates cookie container with Creatio authentication cookies.
	/// </summary>
	/// <returns>Configured CookieContainer for Creatio requests</returns>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Cookie configuration:</listheader>
	///   <item>BPMLOADER, ASPXAUTH, BPMCSRF - main auth cookies</item>
	///   <item>UserType, BPMSESSIONID - session cookies</item>
	///   <item>Special handling for ASPXAUTH domain prefix</item>
	///  </list>
	/// </remarks>
	/// <exception cref="ArgumentException">Invalid Creatio URL or missing required cookies</exception>
	public CookieContainer CreateCookieContainerFromArgs(){
		const string bpmcsrf = "BPMCSRF";
		string[] cookieNames = ["BPMLOADER", "ASPXAUTH", bpmcsrf, "UserType", "BPMSESSIONID"];
		CookieContainer cookieContainer = new();
		string? dnsSafeHost = CreatioUrl?.DnsSafeHost;
		foreach (string cookieName in cookieNames) {
			string? cookieValue = Environment.GetEnvironmentVariable($"TIDE_{cookieName}");
			if (string.IsNullOrWhiteSpace(cookieValue)) {
				continue;
			}
			Cookie cookie = string.Equals(cookieName, "ASPXAUTH", StringComparison.OrdinalIgnoreCase)
				? new Cookie($".{cookieName}", cookieValue, "/", dnsSafeHost)
				: new Cookie(cookieName, cookieValue, "/", dnsSafeHost);

			cookie.HttpOnly = cookie.Name != bpmcsrf;
			cookieContainer.Add(cookie);
		}
		Debug.Assert(cookieContainer.Count >=4, "Missing required cookies");
		return cookieContainer;
	}
	
	#endregion

}