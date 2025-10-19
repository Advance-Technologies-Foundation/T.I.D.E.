using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.SessionState;
using AtfTIDE.HttpClient;
using AtfTIDE.HttpClient.GithubDto;
using Common.Logging;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;
using Terrasoft.Web.Http.Abstractions;


namespace AtfTIDE.WebServices{

    #region Class: Tide

    /// <summary>
    ///     Provides web service functionality for the AtfTIDE system with <c>ConsoleGit.exe</c> integration capabilities.
    ///     This service operates in read-only mode with respect to session state.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>This service class implements WCF service contract</item>
    ///         <item>Requires ASP.NET compatibility mode</item>
    ///         <item>Inherits from BaseService and implements IReadOnlySessionState for secure session handling</item>
    ///     </list>
    /// </remarks>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Tide : BaseService, IReadOnlySessionState{
        #region Methods: Private

        /// <summary>
        ///     Recursively deletes the specified directory and all its files and subdirectories if it exists.
        /// </summary>
        /// <param name="directory">Root directory to delete. If it does not exist the method returns immediately.</param>
        /// <remarks>
        ///     Resets file attributes to Normal before deletion to avoid issues with read-only files.
        ///     Processes subdirectories depth-first. Exceptions are not caught; caller should handle failures.
        /// </remarks>
        private static void DeleteDirectoryRecursively(DirectoryInfo directory) {
            if (!directory.Exists) {
                return;
            }

            // Delete all files
            foreach (FileInfo file in directory.GetFiles()) {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }

            // Recursively delete all subdirectories
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) {
                DeleteDirectoryRecursively(subDirectory);
            }

            // Delete the empty directory
            directory.Delete(false);
        }


        /// <summary>
        ///     Extracts all entries from the specified ZIP archive into the given destination directory,
        ///     recreating the original folder structure.
        /// </summary>
        /// <param name="archivePath">Full path to the ZIP archive to extract.</param>
        /// <param name="destinationPath">Target directory for extracted files. Created if it does not exist.</param>
        /// <remarks>
        ///     Skips directory entries (length == 0) except for ensuring their existence.
        ///     Overwrites existing files unconditionally.
        ///     Caller should ensure the archive is trusted to avoid zip-slip or malicious payload issues.
        /// </remarks>
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

        #endregion

        #region Methods: Public

        /// <summary>
        ///     Captures and processes arguments required for <c>ConsoleGit.exe</c> functionality.
        /// </summary>
        /// <remarks>
        ///     This endpoint is called from
        ///     <strong>
        ///         <c>AtfTIDE_FormPage</c>
        ///     </strong>
        ///     before start of any Business Process.
        ///     It captures system and user information to be used in UserTasks of a process.
        ///     This endpoint performs the following operations:
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Operation Steps</term>
        ///             <description>Detailed process flow</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Request Retrieval</term>
        ///             <description>Retrieves the current HTTP request and its cookies</description>
        ///         </item>
        ///         <item>
        ///             <term>System Information</term>
        ///             <description>Collects system information including the base application URL</description>
        ///         </item>
        ///         <item>
        ///             <term>Framework Detection</term>
        ///             <description>Determines if the system is running in framework mode</description>
        ///         </item>
        ///         <item>
        ///             <term>Data Storage</term>
        ///             <description>Stores the collected information for the current user</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <returns>
        ///     Returns NoContent - 204.
        /// </returns>
        /// <example>
        ///     This endpoint can be called via a GET request:
        ///     <code>
        /// GET <c>rest/Tide/CaptureClioArgs</c> HTTP/1.1
        /// </code>
        /// </example>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public void CaptureClioArgs() {
            Dictionary<string, string> sysInfo = new Dictionary<string, string>();
            string systemUrl = WebUtilities.GetBaseApplicationUrl(HttpContextAccessor.GetInstance().Request);
            sysInfo["IsFramework"]= systemUrl.EndsWith("/0", StringComparison.InvariantCulture) ? "true" : "false";
            sysInfo["SystemUrl"]= systemUrl;
            
            //We no longer use this, we pass credentials stored in sysSettings
            //HttpRequest request = HttpContextAccessor.GetInstance().Request;
            //HttpCookieCollection cookies = request.Cookies;
            // foreach (string cookieName in cookies.Keys) {
            //     sysInfo[cookieName] = cookies[cookieName].Value;
            // }

            HelperFunctions.AddClioArgsForUser(UserConnection.CurrentUser.Id, sysInfo);

#if NETSTANDARD2_0
            HttpResponse response = HttpContextAccessor.GetInstance().Response;
            response.StatusCode = 204;
#else
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
#endif
        }

        /// <summary>
        ///     Discards local (uncommitted) changes for the specified files in a repository using ConsoleGit.
        /// </summary>
        /// <remarks>
        ///     Accepts a JSON payload mapped to <see cref="DiscardFileChangesDto" /> with the target repository ID and an array of
        ///     file paths.
        ///     Forwards a discard command to the underlying git wrapper and returns a simple success marker.
        /// </remarks>
        /// <example>
        ///     POST rest/Tide/DiscardFileChanges
        ///     {
        ///     "repositoryId": "11111111-1111-1111-1111-111111111111",
        ///     "files": ["Schemas/Account/Account.cs", "Schemas/Contact/Contact.cs"]
        ///     }
        /// </example>
        /// <returns>"OK" when the discard command is executed.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public string DiscardFileChanges(DiscardFileChangesDto dto) {
            RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(dto.RepositoryId, UserConnection);
            ConsoleGitArgs args = new ConsoleGitArgs {
                Command = Commands.DiscardFiles, GitUrl = repositoryInfo.GitUrl, Password = repositoryInfo.Password
                , UserName = repositoryInfo.UserName
                , RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString()
                , Files = string.Join(",", dto.Files)
            };


            ConsoleGitResult gitCommandResult = ClassFactory
                .Get<IConsoleGit>("AtfTIDE.ConsoleGit")
                .Execute(args);
            return "OK";
        }

        /// <summary>
        ///     Returns a Git diff for the repository identified by <paramref name="repositoryId" />.
        /// </summary>
        /// <param name="repositoryId">Repository identifier whose working directory changes should be diffed.</param>
        /// <returns>Raw diff text produced by the underlying ConsoleGit execution.</returns>
        /// <remarks>
        ///     Wraps the ConsoleGit GetDiff command. The result is the plain textual diff (unified format).
        /// </remarks>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public string GetDiffForRepository(Guid repositoryId) {
            RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(repositoryId, UserConnection);
            ConsoleGitArgs args = new ConsoleGitArgs {
                Command = Commands.GetDiff, GitUrl = repositoryInfo.GitUrl, Password = repositoryInfo.Password
                , UserName = repositoryInfo.UserName
                , RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString()
            };

            ConsoleGitResult gitCommandResult = ClassFactory
                .Get<IConsoleGit>("AtfTIDE.ConsoleGit")
                .Execute(args);

            return gitCommandResult.Output;
        }


        /// <summary>
        ///     Retrieves GitHub repositories for the specified organization (currently creates a test repository and returns it).
        /// </summary>
        /// <param name="orgName">GitHub organization name whose repositories should be listed.</param>
        /// <returns>List of repositories for the provided organization.</returns>
        /// <example>
        ///     GET <c>rest/Tide/GetRepos?orgName=Creatio-COB</c> HTTP/1.1
        /// </example>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public List<Repository> GetRepos(string orgName) {
            IGitHubClient gh = TideApp.Instance.GetRequiredService<IGitHubClient>();

            //List<Repository> repos = gh.ListOrganizationRepositories(orgName).GetAwaiter().GetResult();
            Repository repo = gh.CreateOrganizationRepository("Creatio-COB", "K-NEW-REPO").GetAwaiter().GetResult();
            List<Repository> repos = new List<Repository> {
                repo
            };
            return repos;
        }

        /// <summary>
        ///     Installs the ConsoleGit tool by extracting it from an embedded archive.
        /// </summary>
        /// <remarks>
        ///     This endpoint extracts the ConsoleGit executable and its dependencies from an embedded archive
        ///     to a designated directory <c>conf/consolegit</c>. It performs cleanup by removing any existing installation before
        ///     extracting the new files.
        /// </remarks>
        /// <returns> Returns the path to the directory where ConsoleGit was installed. </returns>
        /// <example>
        ///     This endpoint can be called via a GET request:
        ///     GET <c>rest/Tide/InstallConsoleGit</c> HTTP/1.1
        /// </example>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public string InstallConsoleGit() {
            ILog logger = LogManager.GetLogger("TIDE");

            string archiveZipPath = HelperFunctions.GetArchivePath();
            string destFolder = HelperFunctions.GetGitConsoleFolderPath();
            logger.Info($"Unzipping archive from {archiveZipPath} to {destFolder}");

            DeleteDirectoryRecursively(new DirectoryInfo(destFolder));
            if (!Directory.Exists(destFolder)) {
                Directory.CreateDirectory(destFolder);
            }

            logger.Info($"Deleted existing directory: {destFolder}");

            string destArchivePath = Path.Combine(destFolder, "archive.zip");
            File.Copy(archiveZipPath, destArchivePath, true);
            UnzipArchive(destArchivePath, destFolder);
            logger.Info($"Unzipped archive to {destFolder}");
            File.Delete(destArchivePath);
            logger.Info($"Deleted temporary archive file: {destArchivePath}");
            return destFolder;
        }

        #endregion
    }

    #endregion

}
