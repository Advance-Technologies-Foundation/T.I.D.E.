using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		var localBranch = InitializedRepository.ListLocalBranches().Value.FirstOrDefault(b => b.FriendlyName == args.BranchName);
		if (localBranch == null) {
			InitializedRepository.Fetch();
			var remoteBranch = InitializedRepository.AllBranches[$"origin/{args.BranchName}"];
			localBranch = InitializedRepository.CreateBranch(args.BranchName, remoteBranch.Tip).Value;
		}
		return InitializedRepository
			   .CheckoutBranch(localBranch.FriendlyName)
			   .MatchFirst<ErrorOr<Success>>(
				   _ => Result.Success,
				   failure => failure
			   );
	}

}
