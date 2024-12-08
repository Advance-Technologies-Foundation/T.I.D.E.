using ErrorOr;

namespace ConsoleGit.Commands;

/// <summary>
/// Make a commit to a non-bare repository
/// </summary>
/// <param name="args">Arguments</param>
/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-commit#make-a-commit-to-a-non-bare-repository"/>
public class CommitCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	public override ErrorOr<Success> Execute() =>
		InitializedRepository
			//TODO: Investigate how we can sign commits with GPG
			.Commit("Kirill Krylov", "k.krylov@creatio.com", "Commit through LibGit2Sharp")
			.MatchFirst<ErrorOr<Success>>(
				_ => Result.Success,
				failure => failure
			);
}

