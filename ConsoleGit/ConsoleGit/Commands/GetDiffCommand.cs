using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

/// <summary>
///  Prints to console changes in index and working directory
/// </summary>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-diff">libgit2sharp Wiki</seealso>
public class GetDiffCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {
	public override ErrorOr<Success> Execute(){
		ErrorOr<string> diff = InitializedRepository.GetDiff();
		if(diff.IsError){
			return Error.Failure("COULD_NOT_GET_DIFF","Could not get diff from repository");
		}
		Console.Out.WriteLine(diff.Value);
		return Result.Success;
	}

}

