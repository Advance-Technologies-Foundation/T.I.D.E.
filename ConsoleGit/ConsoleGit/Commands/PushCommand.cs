using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

/// <summary>
/// Pushes the specified branch to the remote repository.
/// </summary>
/// <param name="args">Arguments</param>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-branch"/>
public class PushCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute(){
		
		ErrorOr<IEnumerable<Branch>> branches = InitializedRepository.ListLocalBranches();
		if(branches.IsError){
			return branches.FirstError;
		}
		
		Branch? currentBranch = branches.Value.FirstOrDefault(b=> b is {IsCurrentRepositoryHead: true, IsRemote: false});
		if(currentBranch == null){
			return Error.Failure("NO_CURRENT_BRANCH","No current branch found");
		}
		
		return InitializedRepository.Push(currentBranch).MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);
	}
}
