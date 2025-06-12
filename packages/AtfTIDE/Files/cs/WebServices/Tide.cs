using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.SessionState;
using AtfTIDE.HttpClient;
using AtfTIDE.HttpClient.GithubDto;
using Common.Logging;
using Terrasoft.Common;
using Terrasoft.Core.Compilation;
using Terrasoft.Core.Factories;
using Terrasoft.Core.Process;
using Terrasoft.Web.Common;
using Terrasoft.Web.Http.Abstractions;

namespace AtfTIDE.WebServices {
	
	/// <summary>
	/// Provides web service functionality for the AtfTIDE system with <c>ConsoleGit.exe</c> integration capabilities.
	/// This service operates in read-only mode with respect to session state.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>This service class implements WCF service contract</item>
	/// <item>Requires ASP.NET compatibility mode</item>
	/// <item>Inherits from BaseService and implements IReadOnlySessionState for secure session handling</item>
	/// </list>
	/// </remarks>
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class Tide : BaseService, IReadOnlySessionState {

		/// <summary>
		/// Captures and processes arguments required for <c>ConsoleGit.exe</c> functionality.
		/// </summary>
		/// <remarks>
		/// This endpoint is called from <strong><c>AtfTIDE_FormPage</c></strong> before start of any Business Process.
		/// It captures system and user information to be used in UserTasks of a process.
		/// This endpoint performs the following operations:
		/// <list type="table">
		/// <listheader>
		/// <term>Operation Steps</term>
		/// <description>Detailed process flow</description>
		/// </listheader>
		/// <item>
		/// <term>Request Retrieval</term>
		/// <description>Retrieves the current HTTP request and its cookies</description>
		/// </item>
		/// <item>
		/// <term>System Information</term>
		/// <description>Collects system information including the base application URL</description>
		/// </item>
		/// <item>
		/// <term>Framework Detection</term>
		/// <description>Determines if the system is running in framework mode</description>
		/// </item>
		/// <item>
		/// <term>Data Storage</term>
		/// <description>Stores the collected information for the current user</description>
		/// </item>
		/// </list>
		/// </remarks>
		/// <returns>
		/// Returns NoContent - 204.
		/// </returns>
		/// <example>
		/// This endpoint can be called via a GET request:
		/// <code>
		/// GET <c>rest/Tide/CaptureClioArgs</c> HTTP/1.1
		/// </code>
		/// </example>
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
					BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public void CaptureClioArgs(){
			HttpRequest request = HttpContextAccessor.GetInstance().Request;
			HttpCookieCollection cookies = request.Cookies;
			Dictionary<string, string> sysInfo = new Dictionary<string, string>();
			string systemUrl = WebUtilities.GetBaseApplicationUrl(HttpContextAccessor.GetInstance().Request);
			sysInfo.Add("IsFramework", systemUrl.EndsWith("/0", StringComparison.InvariantCulture) ? "true" : "false");
			sysInfo.Add("SystemUrl", systemUrl);
			foreach (string cookieName in cookies.Keys) {
				sysInfo.Add(cookieName, cookies[cookieName].Value);
			}
			HelperFunctions.AddClioArgsForUser(UserConnection.CurrentUser.Id, sysInfo);

#if NETSTANDARD2_0
			var response = HttpContextAccessor.GetInstance().Response;
			response.StatusCode = 204;
#else			
			WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
#endif			
			
		}

		
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public string GetDiffForRepository(Guid repositoryId){
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(repositoryId, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.GetDiff,
				GitUrl = repositoryInfo.GitUrl,
				Password = repositoryInfo.Password,
				UserName = repositoryInfo.UserName,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString(),
			};
			
			ConsoleGitResult gitCommandResult = ClassFactory
												.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
												.Execute(args);
			
			return gitCommandResult.Output;
		}
		
		// 0/rest/Tide/InstallConsoleGit
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public string InstallConsoleGit() {
			var logger = LogManager.GetLogger("TIDE");
			
			var archiveZipPath = HelperFunctions.GetArchivePath();
			var destFolder = HelperFunctions.GetGitConsoleFolderPath();
			logger.Info($"Unzipping archive from {archiveZipPath} to {destFolder}");
			
			DeleteDirectoryRecursively(new DirectoryInfo(destFolder));
			if(!Directory.Exists(destFolder)) {
				Directory.CreateDirectory(destFolder);
			}
			
			logger.Info($"Deleted existing directory: {destFolder}");
			
			string destArchivePath = Path.Combine(destFolder, "archive.zip");
			System.IO.File.Copy(archiveZipPath, destArchivePath, true);
			UnzipArchive(destArchivePath, destFolder);
			logger.Info($"Unzipped archive to {destFolder}");
			System.IO.File.Delete(destArchivePath);
			logger.Info($"Deleted temporary archive file: {destArchivePath}");
			return destFolder;
		}
		
		private static void DeleteDirectoryRecursively(DirectoryInfo directory) {
			if (!directory.Exists)
				return;

			// Delete all files
			foreach (FileInfo file in directory.GetFiles()) {
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
		
		
		private static void UnzipArchive(string archivePath, string destinationPath) {
			using (Stream archiveStream = new FileStream(archivePath, FileMode.Open)) {
				using (ZipArchive arch = new ZipArchive(archiveStream, ZipArchiveMode.Read, false)) {
					foreach (ZipArchiveEntry entry in arch.Entries) {
						string fullName = entry.FullName;
						long length = entry.Length;
						string destFilePath = Path.Combine(destinationPath, fullName);
						string dir = Path.GetDirectoryName(destFilePath);
						if (dir != null && !Directory.Exists(dir)) {
							Directory.CreateDirectory(dir);
						}
						if (length > 0) {
							//otherwise it is an empty file
							using (Stream stream = entry.Open()) {
								FileSystem fs = new FileSystem();
								using (FileSystemStream fileStream = fs.File.Create(destFilePath)) {
									stream.CopyTo(fileStream, (int)length);
									fileStream.Flush();
									fileStream.Close();
								}
							}
						}
					}
				}
			}
		}
		
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public string DiscardFileChanges(DiscardFileChangesDto dto){
			
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(dto.RepositoryId, UserConnection);
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.DiscardFiles,
				GitUrl = repositoryInfo.GitUrl,
				Password = repositoryInfo.Password,
				UserName = repositoryInfo.UserName,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString(),
				Files = string.Join(",", dto.Files)
			};
			
			
			ConsoleGitResult gitCommandResult = ClassFactory
												.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
												.Execute(args);
			return "OK";
		}
		
		
		// https://k_krylov_nb.tscrm.com:40031/rest/Tide/GetRepos?orgName=Creatio-COB
		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
		public List<Repository> GetRepos(string orgName) {
			IGitHubClient gh = TideApp.Instance.GetRequiredService<IGitHubClient>();
			//List<Repository> repos = gh.ListOrganizationRepositories(orgName).GetAwaiter().GetResult();
			Repository repo = gh.CreateOrganizationRepository("Creatio-COB","K-NEW-REPO").GetAwaiter().GetResult();
			List<Repository> repos = new List<Repository>() {
				repo
			};
			return repos;
		}
		
	}
	
	[DataContract]
	public class DiscardFileChangesDto {

		[DataMember(Name = "files")]
		public string[] Files { get; set; }
		
		[DataMember(Name = "repositoryId")]
		public Guid RepositoryId { get; set; }

	}
}