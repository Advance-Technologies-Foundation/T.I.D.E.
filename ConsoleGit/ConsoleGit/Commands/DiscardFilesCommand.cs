using System.Text.Json;
using ConsoleGit.Services;
using ErrorOr;
using GitAbstraction;

namespace ConsoleGit.Commands;

/// <summary>
///  Prints to console changes in index and working directory
/// </summary>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-diff">libgit2sharp Wiki</seealso>
public class DiscardFilesCommand(CommandLineArgs args, IWebSocketLogger logger) : BaseRepositoryCommand(args, logger) {
	public override ErrorOr<Success> Execute(){
		
		Args.Files.Split(',').ToList().ForEach(f => InitializedRepository.DiscardFile(f));
		
		ErrorOr<List<ChangedFile>> diff = InitializedRepository.GetChangedFiles();
		if(diff.IsError){
			return Error.Failure("COULD_NOT_GET_DIFF","Could not get diff from repository");
		}
		Console.Out.WriteLine(JsonSerializer.Serialize(diff.Value));
		return Result.Success;
	}

}

