using ConsoleGit.Services;
using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute(){
		
		var localBranch = InitializedRepository.ListLocalBranches().Value.FirstOrDefault(b => b.FriendlyName == Args.BranchName);
		if (localBranch == null) {
			var fetchResult = InitializedRepository.Fetch();
			
			if(fetchResult.IsError) {
				return fetchResult;
			}
			
			var remoteBranch = InitializedRepository.AllBranches[$"origin/{Args.BranchName}"];
			localBranch = InitializedRepository.CreateBranch(Args.BranchName, remoteBranch.Tip).Value;
		}
		return InitializedRepository
			   .CheckoutBranch(localBranch.FriendlyName)
			   .MatchFirst<ErrorOr<Success>>(
				   _ => Result.Success,
				   failure => failure
			   );
	}

}
