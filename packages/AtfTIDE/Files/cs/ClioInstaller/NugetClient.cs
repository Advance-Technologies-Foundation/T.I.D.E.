using System;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AtfTIDE.ClioInstaller.Dto;
using Common.Logging;
using ErrorOr;

namespace AtfTIDE.ClioInstaller {
	public interface INugetClient {

		#region Methods: Public

		/// <summary>
		///  Downloads the latest Clio package asynchronously.
		/// </summary>
		/// <param name="clioDirectory">Directory where clio is to be installed</param>
		/// <returns>An <see cref="ErrorOr{Success}" /> indicating the result of the download operation.</returns>
		/// <remarks>
		///  This method constructs the request URI based on the specified version or retrieves the latest version if not
		///  specified.
		///  It then sends an HTTP GET request to download the package and saves it to a file.
		/// </remarks>
		Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory);

		/// <summary>
		///  Downloads the specific version of Clio package asynchronously.
		/// </summary>
		/// <param name="clioDirectory">Directory where clio is to be installed</param>
		/// <param name="version">
		///  The version of the Clio package to download. If not specified, the latest version
		///  will be downloaded.
		/// </param>
		/// <returns>An <see cref="ErrorOr{Success}" /> indicating the result of the download operation.</returns>
		/// <remarks>
		///  This method constructs the request URI based on the specified version or retrieves the latest version if not
		///  specified.
		///  It then sends an HTTP GET request to download the package and saves it to a file.
		/// </remarks>
		Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory, string version);

		/// <summary>
		///  Searches for the Clio package asynchronously.
		/// </summary>
		/// <returns>
		///  A task that represents the asynchronous operation.
		///  The task result contains an <see cref="ErrorOr{SearchResponse}" /> indicating the
		///  result of the search operation.
		/// </returns>
		Task<ErrorOr<SearchResponse>> SearchAsync();
		
		/// <summary>
		///  Retrieves the maximum version of the specified NuGet package asynchronously.
		/// </summary>
		/// <param name="packageName">The name of the NuGet package.</param>
		/// <returns>
		///  A task that represents the asynchronous operation. The task result contains the maximum version of the
		///  specified package.
		/// </returns>
		Task<ErrorOr<string>> GetMaxVersionAsync(string packageName);

		#endregion

	}

	public class NugetClient : INugetClient {

		#region Fields: Private

		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};

		private readonly System.Net.Http.HttpClient _client;
		private readonly IFileSystem _fileSystem;
		private readonly ILog _logger;

		#endregion

		#region Constructors: Public

		public NugetClient(System.Net.Http.HttpClient client, IFileSystem fileSystem, ILog logger){
			_client = client;
			_fileSystem = fileSystem;
			_logger = logger;
		}

		#endregion

		#region Methods: Private
		
		private async Task<ErrorOr<Success>> DownloadClioInternalAsync(string clioDirectory, Uri routeUrl){
			try {
				HttpResponseMessage response = await _client.GetAsync(routeUrl);
				using (Stream httpStream = await response.Content.ReadAsStreamAsync()) {
					using (ZipArchive arch = new ZipArchive(httpStream, ZipArchiveMode.Read, false)) {
						foreach (ZipArchiveEntry entry in arch.Entries) {
							string fullName = entry.FullName;
							long length = entry.Length;
							string destFilePath = Path.Combine(clioDirectory, fullName);
							string dir = Path.GetDirectoryName(destFilePath);
							if (dir != null && !_fileSystem.Directory.Exists(dir)) {
								_fileSystem.Directory.CreateDirectory(dir);
							}
							if (length > 0) {
								//otherwise it is an empty file
								using (Stream stream = entry.Open()) {
									using (FileSystemStream fileStream = _fileSystem.File.Create(destFilePath)) {
										await stream.CopyToAsync(fileStream, (int)length);
										await fileStream.FlushAsync();
										fileStream.Close();
									}
								}
							}
						}
					}
					_logger.InfoFormat(CultureInfo.InstalledUICulture, "Installed clio to {0}", clioDirectory);
					return Result.Success;
				}
			}
			catch (Exception ex) {
				_logger.ErrorFormat(CultureInfo.InstalledUICulture,
					"En error occured while downloading clio {0}", ex.Message, ex);
				return Error.Failure(ex.Message);
			}
		}

		
		public async Task<ErrorOr<string>> GetMaxVersionAsync(string packageName){
			try {
				string url = $"/v3-flatcontainer/{packageName}/index.json";
				Uri.TryCreate(url, UriKind.Relative, out Uri routeUrl);
				string json = await _client.GetStringAsync(routeUrl);
				using (JsonDocument document = JsonDocument.Parse(json)) {
					JsonElement root = document.RootElement;
					JsonElement versions = root.GetProperty("versions");
					return versions.EnumerateArray().Last().GetString();
				}
			}
			catch(Exception ex) {
				_logger.ErrorFormat(CultureInfo.InstalledUICulture,
					"En error occured while GetMaxVersionAsync clio {0}", ex.Message, ex);
				return Error.Failure(ex.Message);
				
			}
			
		}

		#endregion

		#region Methods: Public

		public async Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory){
			ErrorOr<string> isErrOrMaxVersion = await GetMaxVersionAsync("clio");
			if (isErrOrMaxVersion.IsError) {
				return isErrOrMaxVersion.FirstError;
			}
			string maxVersion = isErrOrMaxVersion.Value;
			string requestUri = $"v3-flatcontainer/clio/{maxVersion}/clio.{maxVersion}.nupkg";
			Uri.TryCreate(requestUri, UriKind.Relative, out Uri routeUrl);
			return await DownloadClioInternalAsync(clioDirectory, routeUrl);
		}

		/// <summary>
		///  Downloads the Clio package asynchronously, and unzips it to clio folder.
		/// </summary>
		/// <param name="clioDirectory">Directory where clio is to be installed</param>
		/// <param name="version">
		///  The version of the Clio package to download. If not specified, the latest version will be
		///  downloaded.
		/// </param>
		/// <returns>An <see cref="ErrorOr{Success}" /> indicating the result of the download operation.</returns>
		public async Task<ErrorOr<Success>> DownloadClioAsync(string clioDirectory, string version){
			string requestUri = $"v3-flatcontainer/clio/{version}/clio.{version}.nupkg";
			Uri.TryCreate(requestUri, UriKind.Relative, out Uri routeUrl);
			return await DownloadClioInternalAsync(clioDirectory, routeUrl);
		}

		public async Task<ErrorOr<SearchResponse>> SearchAsync(){
			const string url = "/query?q=clio&packageType=DotnetTool";
			Uri.TryCreate(url, UriKind.Relative, out Uri routeUrl);
			HttpResponseMessage response = await _client.GetAsync(routeUrl);
			using (Stream str = await response.Content.ReadAsStreamAsync()) {
				return await JsonSerializer.DeserializeAsync<SearchResponse>(str, JsonOptions, CancellationToken.None);
			}
		}

		#endregion

	}
}
