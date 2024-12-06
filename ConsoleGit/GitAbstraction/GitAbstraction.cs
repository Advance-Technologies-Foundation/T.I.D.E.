using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using ErrorOr;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace GitAbstraction {
	public sealed class GitRepository : IDisposable {

		#region Fields: Private

		private bool _disposed;

		#endregion

		#region Constructors: Private

		private GitRepository(UsernamePasswordCredentials cred, Uri gitUrl, string repoDirectory){
			Credentials = cred;
			GitUrl = gitUrl;
			FileSystem = new FileSystem();
			IDirectoryInfoFactory directoryInfoFactory = FileSystem.DirectoryInfo;
			RepoDirectory = directoryInfoFactory.New(repoDirectory);
		}

		#endregion

		#region Properties: Private

		private UsernamePasswordCredentials Credentials { get; }

		private CredentialsHandler CredentialsProvider => (url, user, types) => Credentials;

		private IFileSystem FileSystem { get; }

		private Uri GitUrl { get; }

		private Repository InitializedRepository => RepositoryLazy.Value;

		public IDirectoryInfo RepoDirectory { get; }

		private Lazy<Repository> RepositoryLazy => new Lazy<Repository> (()=>new Repository(RepoDirectory.FullName));

		#endregion

		#region Methods: Private

		private void Dispose(bool disposing){
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
		public static GitRepository GetInstance(string username, string password, Uri gitUrl, string repoDirectory){
			UsernamePasswordCredentials usernamePasswordCredentials = new () {Username = username, Password = password};
			return new GitRepository(usernamePasswordCredentials, gitUrl, repoDirectory);
		}

		/// <summary>
		///  Clones the repository to the specified directory.
		/// </summary>
		/// <returns>
		///  An <see cref="ErrorOr{T}" /> containing the path to the cloned repository or an error.
		/// </returns>
		/// <seealso href="https://github.com/libgit2/libgit2sharp/wiki/git-clone">libgit2sharp Wiki Clone</seealso>
		public ErrorOr<string> Clone(){
			try {
				if(RepoDirectory.Exists){
					RepoDirectory.Delete(true);
				}else{
					RepoDirectory.Create();
				}
				CloneOptions cloneOptions = new CloneOptions();
				if (Credentials == null) {
					return Repository.Clone(GitUrl.ToString(), RepoDirectory.FullName);
				}
				cloneOptions.FetchOptions.CredentialsProvider = CredentialsProvider;
				return Repository.Clone(GitUrl.ToString(), RepoDirectory.FullName, cloneOptions);
			} catch (Exception e) {
				return e.ToError();
			}
		}

		public void Dispose(){
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
		public ErrorOr<IEnumerable<Branch>> ListLocalBranches(){
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
		public ErrorOr<MergeResult> Pull(){
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
		private ErrorOr<Success> Push(string branchName){
			Branch branch  = InitializedRepository.Branches[branchName];
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
		private  ErrorOr<Success> Push(Branch branch){
			PushOptions options = new PushOptions {
				CredentialsProvider = CredentialsProvider
			};
			Remote remote = InitializedRepository.Network.Remotes["origin"];
			InitializedRepository.Network.Push(remote, $"refs/heads/{branch.FriendlyName}", options);
			return Result.Success;
		}
	
	
		public ErrorOr<Success> CheckoutBranch(string branchName){
			Branch branches = InitializedRepository.Branches[branchName];
			if (branches == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}
			var result = Commands.Checkout(InitializedRepository, branchName);
			if (result == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}
			return Result.Success;
		}
	
	
		public ErrorOr<IEnumerable<Commit>> GetCommits(){
		
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
		public ErrorOr<Branch> CreateBranch(string branchName){
			try {
				return InitializedRepository.CreateBranch(branchName);
			} catch (Exception e) {
				return e.ToError();
			}
		}
	
		public ErrorOr<Success> CreateBranchAndPublish(string branchName){
			ErrorOr<Branch> branchOrError = CreateBranch(branchName);
		
			if(branchOrError.IsError){
				return branchOrError.Errors;
			}
			Branch branch = branchOrError.Value;
			InitializedRepository.Branches.Update(branch);
			return Push(branch);
		}
		public ErrorOr<Success> DeleteBranch(string branchName){
			Branch branches = InitializedRepository.Branches[branchName];
			if (branches == null) {
				return Error.Failure("BranchNotFound", $"Branch {branchName} not found");
			}
			InitializedRepository.Branches.Remove(branches);
			return Result.Success;
		}
	
	
		#endregion

		~GitRepository(){
			Dispose(false);
		}

	}
}