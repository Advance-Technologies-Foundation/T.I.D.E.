using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

public class CloneCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute(){
		CleanRepositoryDirectory();
		return InitializedRepository.Clone().MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);
	}

}
