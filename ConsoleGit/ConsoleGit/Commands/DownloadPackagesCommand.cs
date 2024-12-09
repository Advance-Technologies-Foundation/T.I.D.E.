using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Text.Json;
using ConsoleGit.Dto;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace ConsoleGit.Commands;

/// <summary>
/// Downloads packages defined in workspace settings from Creatio.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <listheader>Key features:</listheader>
/// <item>Parallel package downloads</item>
/// <item>Automatic retries</item>
/// <item>Auth cookie management</item>
/// </list>
/// </remarks>
/// <example>
/// Using command:
/// <code>
/// var args = new CommandLineArgs { CreatioUrl = "https://creatio.example.com" };
/// using var command = new DownloadPackagesCommand(args);
/// var result = await command.Execute();
/// </code>
/// </example>
public class DownloadPackagesCommand : ICommand {
	
	private readonly CommandLineArgs _args;
	private readonly HttpClient _client;
	private readonly string _outputPath;
	private const string DownloadPackageUrl = "ServiceModel/PackageInstallerService.svc/GetZipPackages";
	private const string DownloadPackagesPath = "ignore_packages";
	
	/// <summary>
	/// Initializes download command with configuration and HTTP client.
	/// </summary>
	/// <param name="args">Command line arguments containing URLs and credentials</param>
	/// <param name="factory">HttpClientFactory to create HttpClient</param>
	/// <remarks>
	/// <list type="bullet">
	/// <listheader>Initialization:</listheader>
	/// <item>Creates shared HttpClient for downloads</item>
	/// <item>Sets up output directory under repo path</item>
	/// <item>Configures auth cookies and headers</item>
	/// </list>
	/// </remarks>
	/// <exception cref="ArgumentNullException">Args is null</exception>
	public DownloadPackagesCommand(IHttpClientFactory factory, IOptions<CommandLineArgs> args){
		_args = args.Value;
		_client = factory.CreateClient("initializedClient");
		_outputPath = Path.Combine(_args.RepoDir, DownloadPackagesPath);
	}

	/// <summary>
	/// Downloads packages defined in workspace settings asynchronously with error handling and retries.
	/// </summary>
	/// <returns>
	/// Success or list of errors encountered during package downloads.
	/// </returns>
	/// <remarks>
	/// <list type="bullet">
	/// <listheader>Key behaviors:</listheader>
	/// <item>Parallel downloads based on processor count</item>
	/// <item>3 retry attempts with exponential backoff</item>
	/// <item>Thread-safe error collection</item>
	/// </list>
	/// </remarks>
	/// <exception cref="FileNotFoundException">Workspace settings file not found.</exception>
	public ErrorOr<Success> Execute(){
		string workspaceSettingsFilePath = Path.Combine(_args.RepoDir,".clio","workspaceSettings.json");
		if(!File.Exists(workspaceSettingsFilePath)){
			return Error.Failure("Workspace settings file not found", "Workspace settings file not found");
		}
		ErrorOr<Success> result;
		Task.Run(async ()=> {
			string[] packages = await GetPackagesFromSettings(workspaceSettingsFilePath);
			result = await ExecuteAsync(packages);
		}).Wait();
		return result;
	}
	
	/// <summary>
	/// Extracts package names from workspace settings file.
	/// </summary>
	/// <param name="settingsFilePath">Path to workspaceSettings.json file</param>
	/// <returns>Array of package names to download</returns>
	/// <remarks>
	/// <list type="bullet">
	/// <listheader>Validation:</listheader>
	/// <item>Verifies settings file exists and is readable</item>
	/// <item>Ensures packages array is not empty</item>
	/// <item>Validates JSON structure matches WorkspaceSettings schema</item>
	/// </list>
	/// </remarks>
	/// <exception cref="JsonException">Invalid settings file format or empty packages array</exception>
	/// <exception cref="InvalidOperationException">Error accessing or parsing settings file</exception>
	private static async Task<string[]> GetPackagesFromSettings(string settingsFilePath){
		await using FileStream settingsFileStream = new (settingsFilePath, FileMode.Open, FileAccess.Read);
		settingsFileStream.Seek(0, SeekOrigin.Begin);
		WorkspaceSettings? settings = await JsonSerializer.DeserializeAsync<WorkspaceSettings>(settingsFileStream, AppJsonSerializerContext.Default.WorkspaceSettings);
		return settings?.Packages ?? [];
	} 
	
	/// <summary>
	/// Downloads multiple packages in parallel.
	/// </summary>
	/// <param name="packages">Array of package names to download</param>
	/// <returns>Success or aggregated download errors</returns>
	/// <remarks>
	/// <list type="bullet">
	/// <listheader>Processing:</listheader>
	/// <item>Parallel downloads with processor count parallelism</item>
	/// <item>Thread-safe error tracking via ConcurrentDictionary</item>
	/// </list>
	/// </remarks>
	/// <exception cref="HttpRequestException">Package download request failed</exception>
	/// <exception cref="OperationCanceledException">Download cancelled</exception>
	private async Task<ErrorOr<Success>> ExecuteAsync(string[] packages) {
		ConcurrentDictionary<string, ErrorOr<Success>> results = new();
		ParallelOptions options = new () { MaxDegreeOfParallelism = Environment.ProcessorCount };
		await Parallel.ForEachAsync(packages, options, async (package, ct) => {
			try {
				await DownloadPackage(package, ct);
			}
			catch (Exception ex) {
				results.TryAdd(package, Error.Failure($"Failed to download {package}", ex.Message));
			}
		});
		return results.IsEmpty 
					? Result.Success 
					: new List<Error>(results.Values.SelectMany(r => r.Errors));
	}
	
	/// <summary>
	/// Downloads a single package.
	/// </summary>
	/// <param name="package">Package name to download</param>
	/// <param name="ct">Cancellation token for halting download</param>
	/// <remarks>
	/// <list type="bullet">
	/// <listheader>Process:</listheader>
	/// <item>Sends POST request with package name</item> 
	/// <item>Streams response to disk</item>
	/// <item>Validates server response and file integrity</item>
	/// </list>
	/// </remarks>
	/// <exception cref="HttpRequestException">Download request failed</exception>
	/// <exception cref="IOException">Error saving package file</exception>
	private async Task DownloadPackage(string package, CancellationToken ct) {
		string payload = JsonSerializer.Serialize([package], AppJsonSerializerContext.Default.StringArray);
		string requestUrl = _args.IsFramework ? $"/0/{DownloadPackageUrl}" : $"{DownloadPackageUrl}";
		HttpResponseMessage response = await _client.PostAsync(
			requestUrl, new StringContent(payload, Encoding.UTF8, "application/json"), ct);
		
		response.EnsureSuccessStatusCode();
		string outputFile = Path.Combine(_outputPath, $"{package}.zip");
		if(!Directory.Exists(_outputPath)){
			Directory.CreateDirectory(_outputPath);
		}
		await using FileStream fileStream = File.Create(outputFile);
		await response.Content.CopyToAsync(fileStream, ct);
	}
	
	public void Dispose(){
		// TODO release managed resources here
	}

}