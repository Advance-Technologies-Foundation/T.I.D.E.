using ErrorOr;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository
			//ERROR: Need to pass branch here
			.CheckoutBranch("") 
			.MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);

}
