using ErrorOr;

namespace ConsoleGit;

public record CommandLineArgs {

	public CommandLineArgs(){
		Command = Environment.GetEnvironmentVariable(nameof(Command)) ?? "";
		GitUrl = new Uri(Environment.GetEnvironmentVariable(nameof(GitUrl)) ?? "");
		UserName = Environment.GetEnvironmentVariable(nameof(UserName)) ?? "";
		Password = Environment.GetEnvironmentVariable(nameof(Password)) ?? "";
		RepoDir = Environment.GetEnvironmentVariable(nameof(RepoDir)) ?? "";
		CommitMessage = Environment.GetEnvironmentVariable(nameof(CommitMessage)) ?? "";
		CommitAuthorName = Environment.GetEnvironmentVariable(nameof(CommitAuthorName)) ?? "";
		CommitAuthorEmail = Environment.GetEnvironmentVariable(nameof(CommitAuthorEmail)) ?? "";
	}
	
	public string Command { get; init; }
	public Uri GitUrl { get; init; }
	public string UserName { get; init; }
	public string Password { get; init; }
	public string RepoDir { get; init; }
	public string CommitMessage { get; set; }
	public string CommitAuthorName { get; set; }
	public string CommitAuthorEmail { get; set; }
}





