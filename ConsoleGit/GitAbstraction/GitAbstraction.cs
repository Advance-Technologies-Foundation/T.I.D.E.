using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using ErrorOr;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace GitAbstraction
{
	public sealed class GitRepository : IDisposable {

		#region Fields: Private

		private bool _disposed;

		#endregion

		#region Constructors: Private

		private GitRepository(UsernamePasswordCredentials cred, Uri gitUrl, string repoDirectory) {
			Credentials = cred;
			GitUrl = gitUrl;
			FileSystem = new FileSystem();
			IDirectoryInfoFactory directoryInfoFactory = FileSystem.DirectoryInfo;
			RepoDirectory = directoryInfoFactory.New(repoDirectory);
		}

		#endregion

		#region Properties: Private

		public BranchCollection AllBranches => InitializedRepository.Branches;
		private UsernamePasswordCredentials Credentials { get; }

		private CredentialsHandler CredentialsProvider => (url, user, types) => Credentials;

		private IFileSystem FileSystem { get; }

		private Uri GitUrl { get; }

		private Repository InitializedRepository => RepositoryLazy.Value;

		public IDirectoryInfo RepoDirectory { get; }

		private Lazy<Repository> RepositoryLazy => new Lazy<Repository>(() => new Repository(RepoDirectory.FullName));

		private bool IsRepositoryReady => RepoDirectory.Exists && (Repository.IsValid(RepoDirectory.FullName) ||
			Repository.IsValid(Path.Combine(RepoDirectory.FullName, ".git")));

		#endregion

		#region Methods: Private

		private void Dispose(bool disposing) {
			if (!_disposed) {
				if (disposing) {
					// Dispose managed resources
					InitializedRepository.Dispose();
				}
				// Dispose unmanaged resources
				if (RepositoryLazy.IsValueCreated) {
					RepositoryLazy.Value.Dispose();
				}
				_disposed = true;
			}
		}

		#endregion

		#region Methods: Public


		/// <summary>
		///  Gets an instance of the <see cref="GitRepository"/> class.
		/// </summary>
		/// <param name="username">The username for the repository credentials.</param>
		/// <param name="password">The password for the repository credentials.</param>
		/// <param name="gitUrl">The URL of the Git repository.</param>
		/// <param name="repoDirectory">The directory where the repository will be cloned.</param>
		/// <returns>A new instance of the <see cref="GitRepository"/> class.</returns>
		public static GitRepository GetInstance(string username, string password, Uri gitUrl, string repoDirectory) {
			UsernamePasswordCredentials usernamePasswordCredentials = new UsernamePasswordCredentials() { Username = username, Password = password };
			return new GitRepository(usernamePasswordCredentials, gitUrl, repoDirectory);
		}
		
		
		private static void DeleteDirectoryRecursively(IDirectoryInfo directory) {
			if (!directory.Exists)
				return;

			// Delete all files
			foreach (IFileInfo file in directory.GetFiles()) {
				file.Attributes = System.IO.FileAttributes.Normal;
				file.Delete();
			}

			// Recursively delete all subdirectories
			foreach (var subDirectory in directory.GetDirectories()) {
				DeleteDirectoryRecursively(subDirectory);
			}

			// Delete the empty directory
			directory.Delete(false);
		}
		
		
		
		/// <summary>
		///  Clones the repository to the specified directory.
		/// </summary>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> containing the path to the cloned repository or an error.
		/// </returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-clone">libgit2sharp Wiki Clone</seealso>
		public ErrorOr<string> Clone() {
			if (RepoDirectory.Exists) {
				DeleteDirectoryRecursively(RepoDirectory);
				//RepoDirectory.Delete(true);
			} else {
				RepoDirectory.Create();
			}
			
			CloneOptions cloneOptions = new CloneOptions() {
				Checkout = true,
				RecurseSubmodules = true,
				FetchOptions = {
					CredentialsProvider = CredentialsProvider,
					Depth = 5, // Shallow clone
					Prune = true,
				}
			};
			try {
				if (Credentials == null) {
					return Repository.Clone(GitUrl.ToString(), RepoDirectory.FullName);
				}
				cloneOptions.FetchOptions.CredentialsProvider = CredentialsProvider;
				string result = Repository.Clone(GitUrl.ToString(), RepoDirectory.FullName, cloneOptions);
				return result;
			} catch (Exception e) {
				string errorMessage = $"""
									Failed to clone repository to: {RepoDirectory.FullName}
									Permissions: {GetAccessPermissionsForFolder(RepoDirectory.FullName)}
									Exception Message: {e.Message}
									Stack Trace: {e.StackTrace}
									""";
				return Error.Failure("CloneError", errorMessage);
			}
		}

		private static  string GetAccessPermissionsForFolder(string repoDir) {
			DirectoryInfo directoryInfo = new DirectoryInfo(repoDir);
			if(directoryInfo.Exists) {
				if(Environment.OSVersion.Platform == PlatformID.Win32NT) {
					// On Windows, we can get the access control list
					DirectorySecurity accessControl = directoryInfo.GetAccessControl();
					AuthorizationRuleCollection rules = accessControl.GetAccessRules(true, true, typeof(NTAccount));
					StringBuilder sb = new StringBuilder();
					foreach (FileSystemAccessRule rule in rules) {
						sb.AppendLine($"{rule.IdentityReference.Value}: {rule.FileSystemRights} ({rule.AccessControlType})");
					}
					return sb.ToString();
				} else {
					// On Unix-like systems, we can use the standard ls -l command
					return "Obtaining permissions on Unix-like systems is not supported in this implementation. Please check the directory permissions manually.";
				}
			}
			return $"{directoryInfo.FullName} does not exist.";
		}

		public ErrorOr<Success> Fetch() {
			if (!IsRepositoryReady) {
				Clone();
			}
			string logMessage = "";
			try {
				Remote remote = InitializedRepository.Network.Remotes["origin"];
				IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
				FetchOptions fetchOptions = new() {
					CredentialsProvider = CredentialsProvider,
					Prune = true,   // Automatically prune deleted branches
					Depth = 1       // Shallow fetch
				};

				Commands.Fetch(InitializedRepository, remote.Name, refSpecs, fetchOptions, logMessage);
				Console.Out.WriteLine(logMessage);
				return Result.Success;
			} catch (Exception e) {
				Console.Error.WriteLine($"Error fetching from remote repository: {e.Message}");
				return Error.Failure("FetchError", $"Failed to fetch from remote repository: {logMessage}\r\n {e.Message}");
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///  Lists all local branches in the repository.
		/// </summary>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-branch#listing-branches">
		///  libgit2sharp Wiki
		///  listing-branches
		/// </seealso>
		public ErrorOr<IEnumerable<Branch>> ListLocalBranches() {
			try {
				return InitializedRepository.Branches
					//.Where(b => !b.IsRemote)
					.Select(b => b)
					.ToErrorOr();
			} catch (Exception e) {
				return e.ToError();
			}
		}

		/// <summary>
		///  Pulls the latest changes from the remote repository and merges them into the local repository.
		/// </summary>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> containing the result of the merge or an error.
		/// </returns>
		/// ///
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-pull">libgit2sharp Wiki git-pull</seealso>
		public ErrorOr<MergeResult> Pull() {
			// Credential information to fetch
			PullOptions options = new PullOptions() {
				FetchOptions = new FetchOptions {
					CredentialsProvider = CredentialsProvider
				}
			};

			// User information to create a merge commit
			Identity identity = new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL");
			Signature signature = new Signature(identity, DateTimeOffset.Now);

			// Pull
			return Commands.Pull(InitializedRepository, signature, options);
		}

		/// <summary>
		///  Pushes the specified branch to the remote repository.
		/// </summary>
		/// <param name="branchName">The branch to push.</param>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> indicating success or failure.
		/// </returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-push#git-push">libgit2sharp Wiki git-push</seealso>
		public ErrorOr<Success> Push(string branchName) {
			Branch branch = InitializedRepository.Branches[branchName];
			return branch == null ? Error.Failure("BranchNotFound", $"Branch {branchName} not found") : Push(branch);
		}


		/// <summary>
		///  Pushes the specified branch to the remote repository.
		/// </summary>
		/// <param name="branch">The branch to push.</param>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> indicating success or failure.
		/// </returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-push#git-push">libgit2sharp Wiki git-push</seealso>
		public ErrorOr<Success> Push(Branch branch) {
			PushOptions options = new PushOptions() {
				CredentialsProvider = CredentialsProvider
			};
			try {
				InitializedRepository.Network.Push(branch, options);
				return Result.Success;
			}
			catch (Exception e) {
				string reason = e.Message switch {
					{ } msg when msg.Contains("unexpected http status code: 403") => $"{msg}, most likely reason is protected branch or insufficient access rights to push to the repository.",
				    var _ => e.Message
				};
				return Error.Failure("PushError", $"Failed to push branch <strong style='color:blue'>{branch.FriendlyName}</strong> to remote repository: <strong>{reason}</strong>");
			}
		}

		/// <summary>
		/// Checkout to existing branch by name
		/// </summary>
		/// <param name="branchName">Branch name to checkout</param>
		/// <returns></returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-checkout#checkout-to-existing-branch-by-name">libgit2sharp Wiki git checkout <c>branch</c></seealso>
		public ErrorOr<Success> CheckoutBranch(string branchName) {
			Branch firstOrDefaultBranch = InitializedRepository.Branches
				.Where(b => b.FriendlyName == branchName)
				.Select(b => b)
				.FirstOrDefault();

			if (firstOrDefaultBranch == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}

			Branch result = Commands.Checkout(InitializedRepository, firstOrDefaultBranch);
			if (result == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}
			return Result.Success;
		}

		/// <summary>
		///  Stage all working directory changes
		///  Adds all files to the staging area.
		/// </summary>
		/// <remarks>git add --all .</remarks>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-add#stage-all-working-directory-changes">libgit2sharp Wiki git add</seealso>
		public ErrorOr<Success> AddAll() {
			Commands.Stage(InitializedRepository, "*");
			return Result.Success;
		}

		/// <summary>
		/// Stage (add) a file in a repository that is checked out to the file system:
		/// </summary>
		/// <param name="filePath">Filepath to a file with changes, relative to .git directory</param>
		/// <returns></returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-add#add-to-a-non-bare-repository">libgit2sharp Wiki git add</seealso>
		public ErrorOr<Success> AddFile(string filePath) {
			InitializedRepository.Index.Add(filePath);
			InitializedRepository.Index.Write();
			return Result.Success;
		}

		/// <summary>
		/// Make a commit to a non-bare repository
		/// </summary>
		/// <param name="name">Committer name</param>
		/// <param name="email">Committer email</param>
		/// <param name="message">Commit message</param>
		/// <returns><see cref="Commit"/></returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-commit#make-a-commit-to-a-non-bare-repository">libgit2sharp Wiki git commit</seealso>
		public ErrorOr<Commit> Commit(string name, string email, string message) {
			// Create the committer's signature and commit

			Signature author = new Signature(name, email, DateTime.Now);

			// Commit to the repository
			//INVESTIGATE: We can potentially pass TIDE as committer

			Commit commit = InitializedRepository.Commit(message, author, author, new CommitOptions {
				PrettifyMessage = false
			});
			return commit;
		}


		public ErrorOr<IEnumerable<Commit>> GetCommits() {

			return InitializedRepository.Commits.ToList();
		}

		/// <summary>
		///  Creates a new branch pointing at the current HEAD.
		/// </summary>
		/// <param name="branchName">The name of the new branch.</param>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> indicating success or failure.
		/// </returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-branch#creating-a-branch-pointing-at-the-current-head">libgit2sharp Wiki</seealso>
		public ErrorOr<Branch> CreateBranch(string branchName, Commit tip = null) {
			try {
				return tip == null ? InitializedRepository.CreateBranch(branchName) : InitializedRepository.CreateBranch(branchName, tip);
			} catch (Exception e) {
				return e.ToError();
			}
		}


		public ErrorOr<Success> PublishBranch(Branch branch) {
			InitializedRepository.Branches.Update(branch);
			return Push(branch);
		}
		public ErrorOr<Success> CreateBranchAndPublish(string branchName) {
			ErrorOr<Branch> branchOrError = CreateBranch(branchName);

			if (branchOrError.IsError) {
				return branchOrError.Errors;
			}
			Branch branch = branchOrError.Value;
			InitializedRepository.Branches.Update(branch);
			return Push(branch);
		}
		public ErrorOr<Success> DeleteBranch(string branchName) {
			Branch branches = InitializedRepository.Branches[branchName]; //error
			if (branches == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}
			InitializedRepository.Branches.Remove(branches);
			return Result.Success;
		}

		
		
		/// <summary>
		///  Return changes in index and working directory
		/// </summary>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> indicating success or failure.
		/// </returns>
		/// <seealso a="https://github.com/libgit2/libgit2sharp/wiki/git-diff">libgit2sharp Wiki</seealso>
		public ErrorOr<string> GetDiff(){
#if DEBUG			
			Console.Out.WriteLine("Getting diff L326");
			Console.WriteLine("Press any key to continue");
			Console.ReadKey();
#endif
			StringBuilder sb  = new StringBuilder();
			
			string[] exclusions = {
				"/Files/Bin/",
				"/Files/bin/",
				"/Files/Obj/",
				"/Files/obj/"
			};
			
			var difference = InitializedRepository.Diff.Compare<Patch>(
				InitializedRepository.Head.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory, 
				new []{"packages"});
			
			foreach (PatchEntryChanges changes in difference) {
				bool shouldIgnore = exclusions.Any(i=> changes.Path.Contains(i));
				if(!shouldIgnore) {	
					sb.AppendLine(changes.Patch);
				}
			}
			
			string text = sb.ToString();
			
			return text;
		}
		
		
		/// <summary>
		///  Return changes in working directory
		/// </summary>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> indicating success or failure.
		/// </returns>
		/// <seealso a="https://github.com/libgit2/libgit2sharp/wiki/git-diff">libgit2sharp Wiki</seealso>
		public ErrorOr<List<ChangedFile>> GetChangedFiles(){
#if DEBUG			
			Console.Out.WriteLine("Getting chnaged files L350");
			Console.WriteLine("Press any key to continue");
			Console.ReadKey();
#endif
			List<ChangedFile> changedFiles = new List<ChangedFile>();
			foreach (TreeEntryChanges changes in InitializedRepository.Diff.Compare<TreeChanges>(
						InitializedRepository.Head.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory)) {

				
				
				changedFiles.Add(new ChangedFile(changes.Path, changes.Status.ToString()));
				
			}
			return changedFiles;
		}
		
		#endregion

		~GitRepository() {
			Dispose(false);
		}

		public void DiscardFile(string filePath){
			
				string relativePath = filePath.Replace(InitializedRepository.Info.WorkingDirectory, "").TrimStart(Path.DirectorySeparatorChar);
				FileStatus status = InitializedRepository.RetrieveStatus(relativePath);
 
				switch (status)
				{
					case FileStatus.ModifiedInWorkdir:
					case FileStatus.ModifiedInIndex:
					
					case FileStatus.TypeChangeInWorkdir:
					case FileStatus.TypeChangeInIndex:
					
					case FileStatus.DeletedFromWorkdir:
					case FileStatus.DeletedFromIndex:
						// Revert changes or restore deleted file
						InitializedRepository.CheckoutPaths("HEAD", new[] { relativePath }, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force });
						Console.WriteLine($"File {relativePath} restored.");
						break;
 
					case FileStatus.NewInWorkdir:
					case FileStatus.NewInIndex:
						// Delete the new (untracked) file
						if (File.Exists(Path.Combine(InitializedRepository.Info.WorkingDirectory, filePath)))
						{
							File.Delete(Path.Combine(InitializedRepository.Info.WorkingDirectory, filePath));
							Console.WriteLine($"File {relativePath} deleted");
						}
						break;
 
					default:
						Console.WriteLine($"File {relativePath} unchanged.");
						break;
			}
			
		}

	}
	public class ChangedFile {

		public ChangedFile(string path, string status) {
			Path = path;
			Status = status;
		}

		[JsonPropertyName("Path")]
		public string Path { get; set; }
		
		[JsonPropertyName("Status")]
		public string Status { get; set; }

	}
}
