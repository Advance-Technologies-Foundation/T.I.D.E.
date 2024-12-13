using ErrorOr;

namespace ConsoleGit.Commands;

public class PullCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository.Pull().MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);

}
