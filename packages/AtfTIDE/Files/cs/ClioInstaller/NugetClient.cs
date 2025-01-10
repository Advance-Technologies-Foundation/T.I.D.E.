using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller.Dto;
using ErrorOr;
using Terrasoft.Common;

namespace AtfTIDE.ClioInstaller {
	
	public interface INugetClient {
		Task<ErrorOr<SearchResponse>> SearchAsync();

		/// <summary>
		/// Downloads the Clio package asynchronously.
		/// </summary>
		/// <param name="clioDirectory">Directory where clio is to be installed</param>
		/// <param name="version">The version of the Clio package to download. If not specified, the latest version
		/// will be downloaded.</param>
		/// <returns>An <see cref="ErrorOr{Success}"/> indicating the result of the download operation.</returns>
		/// <remarks>
		/// This method constructs the request URI based on the specified version or retrieves the latest version if not specified.
		/// It then sends an HTTP GET request to download the package and saves it to a file.
		/// </remarks>
		Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory, string version = "");
	}
	
	public class NugetClient : INugetClient {

		private readonly HttpClient _client;
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};

		public NugetClient(IHttpClientFactory clientFactory){
			_client = clientFactory.CreateClient(TideConsts.NugetHttpClientName);
		}

		/// <summary>
		/// Downloads the Clio package asynchronously.
		/// </summary>
		/// <param name="clioDirectory">Directory where clio is to be installed</param>
		/// <param name="version">The version of the Clio package to download. If not specified, the latest version will be downloaded.</param>
		/// <returns>An <see cref="ErrorOr{Success}"/> indicating the result of the download operation.</returns>
		public async Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory, string version = ""){
			string requestUri = "";
			if(string.IsNullOrWhiteSpace(version)) {
				string maxVersion = await GetMaxVersionAsync("clio");
				requestUri = $"v3-flatcontainer/clio/{maxVersion}/clio.{maxVersion}.nupkg";
			} 
			else {
				requestUri = $"v3-flatcontainer/clio/{version}/clio.{version}.nupkg";
			}
			HttpResponseMessage response = await _client.GetAsync(requestUri);
			DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(clioDirectory, ".tmp"));
			string tempZipPath = Path.Combine(tempDir.FullName, "clio.zip");
			using(Stream stream = await response.Content.ReadAsStreamAsync()) {
				File.WriteAllBytes(tempZipPath, stream.GetAllBytes());
			};
			UnzipFile(tempZipPath, tempDir.FullName);
			
			string contentDir = Path.Combine(tempDir.FullName, "tools","net8.0", "any"); 
			CopyDirectory(contentDir, clioDirectory);
			
			tempDir.Delete(true);
			return Result.Success;
		}
		
		
		/// <summary>
		/// Unzips the specified zip file to the given extraction path.
		/// </summary>
		/// <param name="zipFilePath">The path to the zip file to be extracted.</param>
		/// <param name="extractPath">The path where the contents of the zip file will be extracted.</param>
		/// <returns>An <see cref="ErrorOr{Success}"/> indicating the result of the extraction operation.</returns>
		private ErrorOr<Success> UnzipFile(string zipFilePath, string extractPath) {
			try {
				System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);
				return Result.Success;
			} catch (Exception ex) {
				return Error.Failure(ex.Message);
			}
		}
		
		/// <summary>
		/// Retrieves the maximum version of the specified NuGet package asynchronously.
		/// </summary>
		/// <param name="packageName">The name of the NuGet package.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the maximum version of the specified package.</returns>
		private async Task<string> GetMaxVersionAsync(string packageName) {
			string url = $"/v3-flatcontainer/{packageName}/index.json";
			string json = await _client.GetStringAsync(url);
			using (JsonDocument document = JsonDocument.Parse(json)) {
				JsonElement root = document.RootElement;
				JsonElement versions = root.GetProperty("versions");
				return versions.EnumerateArray().Last().GetString();
			}
		}
		
		
		/// <summary>
		/// Copies all files and subdirectories from the source directory to the destination directory.
		/// </summary>
		/// <param name="sourceDir">The path of the source directory.</param>
		/// <param name="destDir">The path of the destination directory.</param>
		/// <remarks>
		/// This method recursively copies all files and subdirectories from the source directory to the destination directory.
		/// If the destination directory does not exist, it will be created.
		/// </remarks>
		public void CopyDirectory(string sourceDir, string destDir)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDir);
			if (!dir.Exists) {
				throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
			}
			DirectoryInfo[] dirs = dir.GetDirectories();
			Directory.CreateDirectory(destDir);
			foreach (FileInfo file in dir.GetFiles()) {
				string targetFilePath = Path.Combine(destDir, file.Name);
				file.CopyTo(targetFilePath);
			}
			foreach (DirectoryInfo subDir in dirs) {
				string newDestinationDir = Path.Combine(destDir, subDir.Name);
				CopyDirectory(subDir.FullName, newDestinationDir);
			}
		}
		
		public async Task<ErrorOr<SearchResponse>> SearchAsync(){
			const string url = "/query?q=clio&packageType=DotnetTool";
			HttpResponseMessage response = await _client.GetAsync(url);
			using (Stream str = await response.Content.ReadAsStreamAsync()){
				return await JsonSerializer.DeserializeAsync<SearchResponse>(str, JsonOptions, CancellationToken.None);
			}
		}
	}
}