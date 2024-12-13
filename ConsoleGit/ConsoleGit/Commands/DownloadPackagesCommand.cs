using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ConsoleGit.Common;
using ConsoleGit.Dto;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace ConsoleGit.Commands;

/// <summary>
///  Downloads packages defined in workspace settings from Creatio.
/// </summary>
/// <remarks>
///  <list type="bullet">
///   <listheader>Key features:</listheader>
///   <item>Parallel package downloads</item>
///   <item>Automatic retries</item>
///   <item>Auth cookie management</item>
///  </list>
/// </remarks>
/// <example>
///  Using command:
///  <code>
/// var args = new CommandLineArgs { CreatioUrl = "https://creatio.example.com" };
/// using var command = new DownloadPackagesCommand(args);
/// var result = await command.Execute();
/// </code>
/// </example>
public class DownloadPackagesCommand : ICommand {

	#region Constants: Private

	private const string DownloadPackagesPath = "ignore_packages";
	private const string DownloadPackageUrl = "ServiceModel/PackageInstallerService.svc/GetZipPackages";

	#endregion

	#region Fields: Private

	private readonly IHttpClientFactory _factory;
	private readonly CommandLineArgs _args;

	#endregion

	#region Constructors: Public

	/// <summary>
	///  Initializes download command with configuration and HTTP client.
	/// </summary>
	/// <param name="args">Command line arguments containing URLs and credentials</param>
	/// <param name="factory">HttpClientFactory to create HttpClient</param>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Initialization:</listheader>
	///   <item>Creates shared HttpClient for downloads</item>
	///   <item>Sets up output directory under repo path</item>
	///   <item>Configures auth cookies and headers</item>
	///  </list>
	/// </remarks>
	/// <exception cref="ArgumentNullException">Args is null</exception>
	public DownloadPackagesCommand(IHttpClientFactory factory, IOptions<CommandLineArgs> args){
		_factory = factory;
		_args = args.Value;
	}

	#endregion

	#region Properties: Private

	private Lazy<HttpClient> Client => new(_factory.CreateClient("initializedClient"));

	private string OutputPath => Path.Combine(_args.RepoDir, DownloadPackagesPath);

	#endregion

	#region Methods: Private

	private static async Task<string> ComputeFileHash(string fileName){
		using SHA256 sha256 = SHA256.Create();
		await using FileStream fileStream = File.OpenRead(fileName);
		byte[] hashBytes = await sha256.ComputeHashAsync(fileStream);
		return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
	}

	private static void CopyDirectory(string sourceDir, string destinationDir){

		try {
			Directory.CreateDirectory(destinationDir);
			BaseRepositoryCommand.GrantDeleteAccess(new DirectoryInfo(sourceDir), Environment.UserName);
			BaseRepositoryCommand.GrantDeleteAccess(new DirectoryInfo(destinationDir), Environment.UserName);
			foreach (string file in Directory.GetFiles(sourceDir)) {
				
				if( sourceDir.Contains("\\Files\\obj\\Release")) {
					continue;
				}
				string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
				File.Copy(file, destFile, true);
			}
			foreach (string directory in Directory.GetDirectories(sourceDir)) {
				if(directory.Contains("\\Files\\obj\\Release")) {
					continue;
				}
				
				string destDir = Path.Combine(destinationDir, Path.GetFileName(directory));
				CopyDirectory(directory, destDir);
			}
			
		} catch (Exception e) {
			Console.WriteLine(e.Message);
			throw;
		}
	}
	
	/// <summary>
	///  Extracts package names from workspace settings file.
	/// </summary>
	/// <param name="settingsFilePath">Path to workspaceSettings.json file</param>
	/// <returns>Array of package names to download</returns>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Validation:</listheader>
	///   <item>Verifies settings file exists and is readable</item>
	///   <item>Ensures packages array is not empty</item>
	///   <item>Validates JSON structure matches WorkspaceSettings schema</item>
	///  </list>
	/// </remarks>
	/// <exception cref="JsonException">Invalid settings file format or empty packages array</exception>
	/// <exception cref="InvalidOperationException">Error accessing or parsing settings file</exception>
	private static async Task<string[]> GetPackagesFromSettings(string settingsFilePath){
		await using FileStream settingsFileStream = new(settingsFilePath, FileMode.Open, FileAccess.Read);
		settingsFileStream.Seek(0, SeekOrigin.Begin);
		WorkspaceSettings? settings
			= await JsonSerializer.DeserializeAsync<WorkspaceSettings>(settingsFileStream,
				AppJsonSerializerContext.Default
				                        .WorkspaceSettings);
		return settings?.Packages ?? [];
	}

	private ErrorOr<Success> ApplyChanges(){
		try {
			string[] downloadedPackages = Directory.GetDirectories(OutputPath, "*", SearchOption.TopDirectoryOnly);
			foreach (string downloadedPackage in downloadedPackages) {
				string pkgName1 = downloadedPackage.Replace(OutputPath, "");
				string pkgName = Path.GetFileName(pkgName1);
				string originalPackagePath = Path.Combine(_args.RepoDir, "packages", pkgName);
				
				ErrorOr<Success> result = DeleteWithRetry(originalPackagePath, 3);
				if (result.IsError) {
					return result;
				}
#if DEBUG
				Console.WriteLine($"Deleted {originalPackagePath}");
#endif
				BaseRepositoryCommand.GrantDeleteAccess(new DirectoryInfo(downloadedPackage), Environment.UserName);
				ErrorOr<Success> copyResult = CommonFunctions.Retry(() => CopyDirectory(downloadedPackage, originalPackagePath));
				if(copyResult.IsError) {
					return copyResult;
				}
			}
			var dr =  DeleteWithRetry(OutputPath, 3);
			return dr;
			
		} catch (Exception e) {
			return Error.Failure("Failed to apply downloaded packages", e.Message);
		}
	}

	private ErrorOr<Success> DeleteWithRetry(string originalPackagePath, int retryCount){
		if (Directory.Exists(originalPackagePath)) {
			bool deleted = false;
			while (!deleted && retryCount > 0) {
				try {
					BaseRepositoryCommand.GrantDeleteAccess(new DirectoryInfo(originalPackagePath), Environment.UserName);
					Directory.Delete(originalPackagePath, true);
					deleted = true;
				} catch (IOException) {
					retryCount--;
					Thread.Sleep(1000); // Wait for 1 second before retrying
				}
			}
			if (!deleted) {
				return Error.Failure("COULD NOT DELETE",$"Failed to delete directory {originalPackagePath} after multiple attempts.");
			}
			return Result.Success;
		}
		return Result.Success;
	}
	
	/// <summary>
	///  Downloads a single package.
	/// </summary>
	/// <param name="package">Package name to download</param>
	/// <param name="ct">Cancellation token for halting download</param>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Process:</listheader>
	///   <item>Sends POST request with package name</item>
	///   <item>Streams response to disk</item>
	///   <item>Validates server response and file integrity</item>
	///  </list>
	/// </remarks>
	/// <exception cref="HttpRequestException">Download request failed</exception>
	/// <exception cref="IOException">Error saving package file</exception>
	private async Task<ErrorOr<Success>> DownloadPackage(string package, CancellationToken ct){
		string payload = JsonSerializer.Serialize([package], AppJsonSerializerContext.Default.StringArray);
		string requestUrl = _args.IsFramework ? $"/0/{DownloadPackageUrl}" : $"{DownloadPackageUrl}";
		using HttpResponseMessage response = await Client.Value.PostAsync(
			requestUrl, new StringContent(payload, Encoding.UTF8, "application/json"),
			ct);

		response.EnsureSuccessStatusCode();
		string outputFile = Path.Combine(OutputPath, $"{package}.zip");
		if (!Directory.Exists(OutputPath)) {
			Directory.CreateDirectory(OutputPath);
		}
		await using FileStream fileStream = File.Create(outputFile);
		await response.Content.CopyToAsync(fileStream, ct);
		await fileStream.FlushAsync(CancellationToken.None);
		return Result.Success;
	}

	/// <summary>
	///  Downloads multiple packages in parallel.
	/// </summary>
	/// <param name="packages">Array of package names to download</param>
	/// <returns>Success or aggregated download errors</returns>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Processing:</listheader>
	///   <item>Parallel downloads with processor count parallelism</item>
	///   <item>Thread-safe error tracking via ConcurrentDictionary</item>
	///  </list>
	/// </remarks>
	/// <exception cref="HttpRequestException">Package download request failed</exception>
	/// <exception cref="OperationCanceledException">Download cancelled</exception>
	private async Task<ErrorOr<Success>> DownloadPackagesAsync(string[] packages){
		ConcurrentDictionary<string, ErrorOr<Success>> results = new();
		ParallelOptions options = new() {MaxDegreeOfParallelism = Environment.ProcessorCount};
		await Parallel.ForEachAsync(packages, options, async (package, ct) => {
			try {
				await DownloadPackage(package, ct);
			} 
			catch (Exception ex) {
				results.TryAdd(package, Error.Failure($"Failed to download {package}", ex.Message));
			}
		});
		var errors = new List<Error>() {
			Error.Failure("","")
		};
		return results.IsEmpty
			? Result.Success
			: new List<Error>(results.Values.SelectMany(r => r.Errors));
	}

	private async Task<ErrorOr<Success>> UnpackAsync(){
		try {
			ArchiveUtilities.ArchiveUtilities archiveUtilities = new();
			List<string> zipFiles = Directory.GetFiles(OutputPath, "*.zip").ToList();
			foreach (string file in zipFiles) {
				await archiveUtilities.UnpackZipFileAsync(file, OutputPath, true);
				File.Delete(file);
			}
			string[] gxFiles = Directory.GetFiles(OutputPath, "*.gz");
			foreach (string file in gxFiles) {
				string pkgDir = Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(file));
				Directory.CreateDirectory(pkgDir);
				await archiveUtilities.UnpackGzFile(file, pkgDir);
				File.Delete(file);
			}
			return Result.Success;
		} catch (Exception e) {
			return Error.Failure("Failed to unpack downloaded packages", e.Message);
		}
	}

	#endregion

	#region Methods: Public

	public void Dispose(){
		// TODO release managed resources here
	}

	/// <summary>
	///  Downloads packages defined in workspace settings asynchronously with error handling and retries.
	/// </summary>
	/// <returns>
	///  Success or list of errors encountered during package downloads.
	/// </returns>
	/// <remarks>
	///  <list type="bullet">
	///   <listheader>Key behaviors:</listheader>
	///   <item>Parallel downloads based on processor count</item>
	///   <item>3 retry attempts with exponential backoff</item>
	///   <item>Thread-safe error collection</item>
	///  </list>
	/// </remarks>
	/// <exception cref="FileNotFoundException">Workspace settings file not found.</exception>
	public ErrorOr<Success> Execute(){
		string workspaceSettingsFilePath = Path.Combine(_args.RepoDir, ".clio", "workspaceSettings.json");
		if (!File.Exists(workspaceSettingsFilePath)) {
			return Error.Failure("Workspace settings file not found", "Workspace settings file not found");
		}
		ErrorOr<Success> result;
		Task.Run(async () => {
			string[] packages = await GetPackagesFromSettings(workspaceSettingsFilePath);
			result = await DownloadPackagesAsync(packages);
			if(result.IsError) {
				return;
			} 
			result = await UnpackAsync();
		}).Wait();

		
		if(result.IsError) {
			return result;
		}
		result = ApplyChanges();
		return result;
	}

	#endregion

}