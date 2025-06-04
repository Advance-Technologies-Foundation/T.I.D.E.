using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

public class CloneCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute(){


		try {
			// Logger.LogAsync(MessageType.INF, $"CleanRepositoryDirectory {Args.RepoDir}")
			// 	.ConfigureAwait(false).GetAwaiter().GetResult();
			//
			//GitAbstraction.GitRepository.ApplyPermissionsOnRepoFolder(Args.RepoDir);
			
			//CleanRepositoryDirectory();
			//Thread.Sleep(1000); // Vova's idea to wait for a second before cloning, to avoid issues with file locks or similar problems.
			
			Logger.LogAsync(MessageType.INF, $"Cloning repository from {Args.GitUrl} to {Args.RepoDir}")
				.ConfigureAwait(false).GetAwaiter().GetResult();
			
			// Logger.LogAsync(MessageType.INF, $"Permissions on {GetAccessPermissionsForFolder(Args.RepoDir)}")
			// 	.ConfigureAwait(false).GetAwaiter().GetResult();
			return InitializedRepository.Clone().MatchFirst<ErrorOr<Success>>(
				_ => Result.Success,
				failure => failure
			);
			
		}
		catch (Exception e) {
			Logger.LogAsync(MessageType.INF, $"Permissions on {Args.RepoDir}{Environment.NewLine}{GetAccessPermissionsForFolder(Args.RepoDir)}")
				.ConfigureAwait(false).GetAwaiter().GetResult();
			
			string dirFullName = Path.Combine(Args.RepoDir, ".git");
			Logger.LogAsync(MessageType.INF, $"Permissions on {dirFullName} {Environment.NewLine} {GetAccessPermissionsForFolder(dirFullName)}")
				.ConfigureAwait(false).GetAwaiter().GetResult();
			
			return Error.Failure("COULD_NOT_CLONE","Could not clone repository\r\n" + e.Message +"\r\n" +e.StackTrace+ "\r\n"+ e?.InnerException?.Message);
		}
	}
}
