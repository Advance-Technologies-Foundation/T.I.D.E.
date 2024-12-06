using ErrorOr;

namespace ConsoleGit.Commands;

public class CloneCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		CleanRepositoryDirectory();
		return InitializedRepository.Clone().MatchFirst<ErrorOr<Success>>(
			success => Result.Success,
			failure => failure
		);
	}

}
