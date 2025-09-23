using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

public class CheckoutCommand(CommandLineArgs args, IWebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute(){
		using var repo = InitializedRepository;
		var localBranch = repo.ListLocalBranches().Value.FirstOrDefault(b => b.FriendlyName == Args.BranchName);
		if (localBranch == null) {
			var fetchResult = repo.Fetch();
			if(fetchResult.IsError) {
				return fetchResult;
			}
			var remoteBranch = repo.AllBranches[$"origin/{Args.BranchName}"];
			localBranch = repo.CreateBranch(Args.BranchName, remoteBranch.Tip).Value;
		}
		string upstreamBranch = $"refs/heads/{Args.BranchName}";
		repo.AllBranches.Update(
			localBranch,
			b => b.Remote = "origin",
			b => b.UpstreamBranch = upstreamBranch
		);
		return repo
				.CheckoutBranch(localBranch.FriendlyName)
				.MatchFirst<ErrorOr<Success>>(
					_ => Result.Success,
					failure => failure
				);
	}
}