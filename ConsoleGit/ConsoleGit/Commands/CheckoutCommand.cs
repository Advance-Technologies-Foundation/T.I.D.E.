using ErrorOr;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		return InitializedRepository.CheckoutBranch("").MatchFirst<ErrorOr<Success>>(
			success => Result.Success,
			failure => failure
		);
	}

}
