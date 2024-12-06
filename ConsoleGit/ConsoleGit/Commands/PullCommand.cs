using ErrorOr;

namespace ConsoleGit.Commands;

public class PullCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		return InitializedRepository.Pull().MatchFirst<ErrorOr<Success>>(
			success => Result.Success,
			failure => failure
		);
	}

}
