using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

public class PullCommand(CommandLineArgs args, IWebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository.Pull().MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);

}
