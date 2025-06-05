using System.Security.AccessControl;
using System.Text;
using ConsoleGit.Services;
using ErrorOr;
using GitAbstraction;
using LibGit2Sharp;

namespace ConsoleGit.Commands;

public interface ICommand : IDisposable {

	#region Methods: Public

	ErrorOr<Success> Execute();

	#endregion

}


public abstract class BaseRepositoryCommand: ICommand {

	#region Fields: Private

	private bool _disposed;
	private readonly Lazy<GitRepository> _initializedRepository;

	#endregion

	#region Fields: Protected

	protected readonly CommandLineArgs Args;
	protected readonly WebSocketLogger Logger;

	#endregion

	#region Constructors: Protected

	protected BaseRepositoryCommand(CommandLineArgs args, WebSocketLogger logger) {
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
		
		Logger.LogAsync(MessageType.INF, $"Cleaning repository directory: {Args.RepoDir}")
			.ConfigureAwait(false).GetAwaiter().GetResult();
		
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

	
	protected string GetAccessPermissionsForFolder(string repoDir) {
		DirectoryInfo directoryInfo = new (repoDir);
		if(directoryInfo.Exists) {
			if(Environment.OSVersion.Platform == PlatformID.Win32NT) {
				// On Windows, we can get the access control list
				DirectorySecurity accessControl = directoryInfo.GetAccessControl();
				AuthorizationRuleCollection rules = accessControl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
				StringBuilder sb = new ();
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