using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		
		
		// Get the remote branch
		var remoteBranch = InitializedRepository.AllBranches[$"origin/{args.BranchName}"];
    
		// Create and checkout a new local branch that tracks the remote branch
		Branch localBranch = InitializedRepository.CreateBranch(args.BranchName, remoteBranch.Tip);
		Commands.Checkout(repo, localBranch);
    
		// Set up tracking information
		repo.Branches.Update(localBranch,
			b => b.TrackedBranch = remoteBranch.CanonicalName);
		
		return InitializedRepository
		       .CheckoutBranch(args.BranchName)
		       .MatchFirst<ErrorOr<Success>>(
			       _ => Result.Success,
			       failure => failure
		       );
	}

}
