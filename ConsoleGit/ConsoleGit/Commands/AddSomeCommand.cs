using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

/// <summary>
/// Stage all working directory changes
/// </summary>
/// <param name="args">Arguments</param>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-add#stage-all-working-directory-changes"/>
public class AddSomeCommand(CommandLineArgs args, IWebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository.AddFiles(Args.Files.Split(',')).MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);
}
