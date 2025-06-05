using ConsoleGit.Services;
using ErrorOr;

namespace ConsoleGit.Commands;

public class CloneCommand(CommandLineArgs args, WebSocketLogger logger) : BaseRepositoryCommand(args, logger) {

	public override ErrorOr<Success> Execute(){
		try {
			return InitializedRepository.Clone().MatchFirst<ErrorOr<Success>>(
				_ => Result.Success,
				failure => failure
			);
		}
		catch (Exception e) {
			if(Directory.Exists(Args.RepoDir)) {
				Logger.LogAsync(MessageType.ERR, $"Permissions on {Args.RepoDir}{Environment.NewLine}{GetAccessPermissionsForFolder(Args.RepoDir)}")
					.ConfigureAwait(false).GetAwaiter().GetResult();
			}else {
				Logger.LogAsync(MessageType.ERR, $"Directory does not exist {Args.RepoDir} {Environment.NewLine} {GetAccessPermissionsForFolder(Args.RepoDir)}")
					.ConfigureAwait(false).GetAwaiter().GetResult();
			}
			return Error.Failure("COULD_NOT_CLONE","Could not clone repository\r\n" + e.Message +"\r\n" +e.StackTrace+ "\r\n"+ e?.InnerException?.Message);
		}
	}
}
