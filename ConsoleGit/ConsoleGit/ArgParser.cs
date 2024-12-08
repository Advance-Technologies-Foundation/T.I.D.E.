using ErrorOr;

namespace ConsoleGit;

public record CommandLineArgs {

	private CommandLineArgs(string Command, Uri GitUrl, string UserName, string Password, string RepoDir){
		this.Command = Command;
		this.GitUrl = GitUrl;
		this.UserName = UserName;
		this.Password = Password;
		this.RepoDir = RepoDir;
	}

	public static readonly Func<string[], ErrorOr<CommandLineArgs>> Parse = args => {
		if (args.Length < 5) {
			return Error.Failure("NOT ENOUGH ARGUMENTS","Not enough arguments", 
				new Dictionary<string, object> { 
					{ "Usage", "<command> <gitUrl> <userName> <password> <repoDir>" }
			});
		}
		
		bool isUrl = Uri.TryCreate(args[1], UriKind.Absolute, out Uri gitUrl);
		
		if(isUrl) {
			return new CommandLineArgs(args[0], gitUrl, args[2], args[3], args[4]);	
		}else {
			return Error.Failure("INVALID_URL","Repository url is invalid");
		}
	};

	public string Command { get; init; }

	public Uri GitUrl { get; init; }

	public string UserName { get; init; }

	public string Password { get; init; }

	public string RepoDir { get; init; }
	
}





