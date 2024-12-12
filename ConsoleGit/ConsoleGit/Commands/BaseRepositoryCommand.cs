using System.Security.AccessControl;
using ErrorOr;
using GitAbstraction;

namespace ConsoleGit.Commands;

public interface ICommand : IDisposable {

	#region Methods: Public

	ErrorOr<Success> Execute();

	#endregion

}

public abstract class BaseRepositoryCommand : ICommand {

	#region Fields: Private

	private bool _disposed;
	private readonly Lazy<GitRepository> _initializedRepository;

	#endregion

	#region Fields: Protected

	protected readonly CommandLineArgs Args;

	#endregion

	#region Constructors: Protected

	protected BaseRepositoryCommand(CommandLineArgs args){
		Args = args;
		_initializedRepository = new Lazy<GitRepository>(() =>
			GitRepository.GetInstance(args.UserName, args.Password, args.GitUrl, args.RepoDir));
	}

	#endregion

	#region Properties: Protected

	protected GitRepository InitializedRepository => _initializedRepository.Value;

	#endregion

	#region Methods: Private

	public static void GrantDeleteAccess(DirectoryInfo directoryInfo, string userName){
		// Get the current directory security settings
		DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

		// Define the delete access rule
		FileSystemAccessRule deleteRule = new(
			userName,
			FileSystemRights.Delete,
			InheritanceFlags.None,
			PropagationFlags.NoPropagateInherit,
			AccessControlType.Allow);

		// Add the rule
		directorySecurity.AddAccessRule(deleteRule);

		// Apply the updated security settings
		directoryInfo.SetAccessControl(directorySecurity);
	}

	#endregion

	#region Methods: Protected

	protected void CleanRepositoryDirectory(){
		if (Directory.Exists(Args.RepoDir)) {
			DirectoryInfo directoryInfo = new(Args.RepoDir);
			GrantDeleteAccess(directoryInfo, Environment.UserName);
			directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ToList().ForEach(file => {
				file.Attributes = FileAttributes.Normal;
				file.Delete();
			});
			Directory.Delete(Args.RepoDir, true);
			Directory.CreateDirectory(Args.RepoDir);
		}
	}

	protected virtual void Dispose(bool disposing){
		if (!_disposed) {
			if (disposing) {
				// Dispose managed resources here.
				_initializedRepository?.Value?.Dispose();
			}
			// Dispose unmanaged resources here.
			_disposed = true;
		}
	}

	#endregion

	#region Methods: Public

	public abstract ErrorOr<Success> Execute();

	public void Dispose(){
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	#endregion

	~BaseRepositoryCommand(){
		Dispose(false);
	}

}