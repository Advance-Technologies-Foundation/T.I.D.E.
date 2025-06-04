using ConsoleGit.Services;
using ErrorOr;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

/// <summary>
/// Stage all working directory changes
/// </summary>
/// <param name="args">Arguments</param>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-add#stage-all-working-directory-changes"/>
public class AddAllCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository.AddAll().MatchFirst<ErrorOr<Success>>(
			_ => Result.Success,
			failure => failure
		);

}
