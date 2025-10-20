using AtfTIDE;
using Terrasoft.Core.Factories;

namespace Terrasoft.Core.Process.Configuration
{
	using Terrasoft.Core.Process;

	#region Class: AtfProcessUserTask_CloneRepository

	/// <include />
	public partial class AtfProcessUserTask_CloneRepository {
	
		/// <summary>
		/// Hey, this is what's running when the process is executed.
		/// </summary>
		/// <param name="context">Creatio will pass context to this method</param>
		/// <returns>Always <c>true</c></returns>
		/// <remarks>
		/// <see href="../UserTask/AtfProcessUserTask_CloneRepository.html">See Wiki</see>
		/// </remarks>
		protected override bool InternalExecute(ProcessExecutingContext context){
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(Repository, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.Clone,
				GitUrl = repositoryInfo.GitUrl,
				Password = repositoryInfo.Password,
				UserName = repositoryInfo.UserName,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString(),
			};

			if (!string.IsNullOrEmpty(repositoryInfo.BranchName)) {
				args.BranchName = repositoryInfo.BranchName;
			}

			ConsoleGitResult gitCommandResult = ClassFactory
				.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
				.Execute(args);
			
			if(gitCommandResult.ExitCode != 0) {
				IsError = true;
				ErrorMessage = gitCommandResult.ErrorMessage;
			}else {
				RepositoryFolderPath = args.RepoDir;
				Output = gitCommandResult.Output;
			}
			return true;
		}
	}
	
	#endregion

}

