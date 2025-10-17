using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using ErrorOr;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace AtfTIDE{

    #region Class: HelperFunctions

    public static class HelperFunctions{
        public static readonly Dictionary<Guid, Dictionary<string, string>> ClioArguments
            = new Dictionary<Guid, Dictionary<string, string>>();

        #region Methods: Private

        /// <summary>
        ///     Fetches the repository entity by its unique identifier.
        /// </summary>
        /// <param name="repositoryId">Unique identifier of the repository record.</param>
        /// <param name="userConnection">
        ///     Optional user connection. If <c>null</c>, a new <see cref="UserConnection" /> is resolved via
        ///     <see cref="ClassFactory" />.
        /// </param>
        /// <returns>
        ///     Repository <see cref="IEntity">entity</see> if found; otherwise <c>null</c>.
        /// </returns>
        private static IEntity FetchRepositoryEntityById(Guid repositoryId, UserConnection userConnection = null) {
            UserConnection uc = userConnection ?? ClassFactory.Get<UserConnection>();
            const string repositorySchemaName = "AtfRepository";
            Entity repositoryEntity = uc.EntitySchemaManager
                .GetInstanceByName(repositorySchemaName)
                .CreateEntity(uc);
            bool isFetched = repositoryEntity.FetchFromDB(repositoryId);
            return !isFetched ? null : repositoryEntity;
        }

        private static string GetApplicationId() {
#if NETSTANDARD
            return Path.GetFileName(Environment.CurrentDirectory).Replace(Path.GetInvalidPathChars(), '_');
#else
            var info = new AspNetAppDomainInfo(AppDomain.CurrentDomain.FriendlyName);
            return info.SiteId.ToString(CultureInfo.InvariantCulture);
#endif
        }


        /// <summary>
        ///     Returns the resolved temporary workspace path derived from the 'tempDirectoryPath' connection string.
        ///     Expands environment variables and replaces placeholders (%APPLICATION%, %APPPOOLIDENTITY%, %USER%, %WORKSPACE%,
        ///     %TEMP% on Unix).
        ///     Returns empty string if the configuration entry is absent.
        /// </summary>
        private static string GetTempPath() {
            UserConnection userConnection = ClassFactory.Get<UserConnection>();
            ConnectionStringSettings connectionStringSettings = userConnection
                .AppConnection
                .AppSettings
                .RootConfiguration
                .ConnectionStrings
                .ConnectionStrings["tempDirectoryPath"];

            if (connectionStringSettings == null) {
                return string.Empty;
            }

            string tempPath = PrepareWorkspacePath(connectionStringSettings.ConnectionString
                , userConnection.AppConnection.Workspace.Name);
            return tempPath;
        }

        private static string PrepareWorkspacePath(string workspacePath, string workspaceName) {
#if NETFRAMEWORK // TODO #CRM-47405
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            string identityName = windowsIdentity.Name;
#else
            string identityName = Environment.UserName;
#endif
            return PrepareWorkspacePath(workspacePath, identityName, workspaceName);
        }

        /// <summary>
        ///     Prepares a workspace path by expanding environment variables and replacing placeholders:
        ///     %APPLICATION%, %APPPOOLIDENTITY%, %USER%, %WORKSPACE% (and %TEMP% on Unix).
        ///     Invalid path or file name characters in user and workspace names are replaced with '_'.
        /// </summary>
        /// <param name="workspacePath">Template path that may contain placeholders and environment variables.</param>
        /// <param name="userName">User (or app pool identity) whose name is injected into the path.</param>
        /// <param name="workspaceName">Workspace name injected into the path.</param>
        /// <returns>Resolved workspace path.</returns>
        private static string PrepareWorkspacePath(string workspacePath, string userName, string workspaceName) {
            const StringComparison comparisonIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
            string correctWorkspaceName = workspaceName.Replace(Path.GetInvalidPathChars(), '_');
            string correctUserName = userName.Replace(Path.GetInvalidFileNameChars(), '_');
            workspacePath = Environment.ExpandEnvironmentVariables(workspacePath);
            workspacePath = workspacePath.Replace("%APPLICATION%", GetApplicationId(), comparisonIgnoreCase);
            workspacePath = workspacePath.Replace("%APPPOOLIDENTITY%", correctUserName, comparisonIgnoreCase);
            workspacePath = workspacePath.Replace("%USER%", correctUserName, comparisonIgnoreCase);
            workspacePath = workspacePath.Replace("%WORKSPACE%", correctWorkspaceName, comparisonIgnoreCase);
            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                workspacePath = workspacePath.Replace("%TEMP%", Path.GetTempPath(), comparisonIgnoreCase);
            }

            return workspacePath;
        }

        #endregion

        #region Methods: Public

        /// <summary>
        ///     Adds or updates the stored Clio command line arguments for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose arguments are being set.</param>
        /// <param name="args">Dictionary of argument key/value pairs to associate with the user.</param>
        public static void AddClioArgsForUser(Guid userId, Dictionary<string, string> args) {
            if (ClioArguments.ContainsKey(userId)) {
                ClioArguments[userId] = args;
            }
            else {
                ClioArguments.Add(userId, args);
            }
        }

        /// <summary>
        ///     Creates (if absent) and returns a temporary directory used for transient .NET tooling artifacts.
        ///     The directory path is constructed as: GetClioDirectory()/.dotnet_tmp.
        /// </summary>
        /// <remarks>
        ///     Ensures the directory exists before returning the DirectoryInfo instance.
        /// </remarks>
        /// <returns><see cref="DirectoryInfo" /> representing the temporary working directory.</returns>
        public static DirectoryInfo CreateTempDirectory() {
            DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(GetClioDirectory().FullName, ".dotnet_tmp"));
            if (!tempDir.Exists) {
                tempDir.Create();
            }

            return tempDir;
        }

        /// <summary>
        ///     Fetches the repository name (column AtfName) by its unique identifier.
        /// </summary>
        /// <param name="repositoryId">Unique identifier of the repository record.</param>
        /// <param name="userConnection">
        ///     Optional user connection. If <c>null</c>, a new <see cref="UserConnection" /> is resolved via
        ///     <see cref="ClassFactory" />.
        /// </param>
        /// <returns>
        ///     Repository name if found; otherwise an empty string.
        /// </returns>
        public static string FetchRepositoryName(Guid repositoryId, UserConnection userConnection = null) {
            IEntity repositoryEntity = FetchRepositoryEntityById(repositoryId, userConnection);
            if (repositoryEntity == null) {
                return string.Empty;
            }

            string name = repositoryEntity.GetTypedColumnValue<string>("AtfName");
            return name ?? string.Empty;
        }

        /// <summary>
        ///     Returns the absolute path to the packaged archive file (archive.zip) located under
        ///     <c>Terrasoft.Configuration/Pkg/AtfTIDE/Files/exec/archive.zip</c>.
        /// </summary>
        /// <returns>The full path to archive.zip (its existence is not guaranteed).</returns>
        /// <remarks>This zip archive contains ConsoleGit binaries</remarks>
        public static string GetArchivePath() {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string execFilePath = Path.Combine(baseDir, "Terrasoft.Configuration", "Pkg", "AtfTIDE", "Files", "exec"
                , "archive.zip");
            return execFilePath;
        }

        /// <summary>
        ///     Gets the directory information for the Clio installation.
        /// </summary>
        /// <returns>
        ///     A <see cref="DirectoryInfo" /> object representing the Clio directory.
        /// </returns>
        public static DirectoryInfo GetClioDirectory() {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string clioDirectory = Path.Combine(baseDir, "conf", "clio");
            return new DirectoryInfo(clioDirectory);
        }

        /// <summary>
        ///     Returns the full path to the <c>clio.dll</c> file within the Clio directory (searched recursively).
        /// </summary>
        /// <returns>
        ///     Absolute path to <c>clio.dll</c>; empty string if not found.
        /// </returns>
        public static string GetClioFilePath() {
            DirectoryInfo cliodir = GetClioDirectory();
            return cliodir.GetFiles("clio.dll", SearchOption.AllDirectories).FirstOrDefault()?.FullName ?? string.Empty;
        }

        /// <summary>
        ///     Returns the absolute path to the ConsoleGit.dll file.
        /// </summary>
        /// <returns>The full path to the ConsoleGit.dll file.</returns>
        /// <remarks>This file contains ConsoleGit binaries after installation</remarks>
        public static ErrorOr<FileInfo> GetConsoleGitPath() {
            string execFilePath = Path.Combine(GetGitConsoleFolderPath(), "ConsoleGit.dll");

            // Validate executable path exists
            if (!File.Exists(execFilePath)) {
                return Error.Failure("Process error", "Executable not found");
            }

            return new FileInfo(execFilePath);
        }

        /// <summary>
        ///     Returns the absolute path to the ConsoleGit folder.
        /// </summary>
        /// <returns>The full path to the ConsoleGit folder.</returns>
        /// <remarks>This folder contains ConsoleGit binaries after installation</remarks>
        public static string GetGitConsoleFolderPath() {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string execFolderPath = Path.Combine(baseDir, "conf", "consolegit");
            return execFolderPath;
        }

        /// <summary>
        ///     Returns (creating if necessary) a temporary workspace directory for the specified repository.
        /// </summary>
        /// <param name="repositoryName">Logical repository name; used as the folder name.</param>
        /// <returns>DirectoryInfo pointing to the repository workspace directory.</returns>
        /// <remarks>
        ///     Root path is derived from the configured temp directory (GetTempPath()).
        ///     The directory is created if it does not already exist.
        /// </remarks>
        public static DirectoryInfo GetRepositoryDirectory(string repositoryName) {
            // string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // string repositoryDirectory = Path.Combine(baseDir, "conf","tide", repositoryName);
            // string repositoryDirectory = Path.Combine(baseDir, "Terrasoft.Configuration","Pkg", repositoryName);
            // string repositoryDirectory = Path.Combine("C:\\", "Windows","Temp","60","MS_iis-267b870a97d8435d","Default", repositoryName);
            string repositoryDirectory = Path.Combine(GetTempPath(), repositoryName);
            Directory.CreateDirectory(repositoryDirectory);
            return new DirectoryInfo(repositoryDirectory);
        }

        /// <summary>
        ///     Fetches the repository information (column AtfName, AtfRepositoryUrl, AtfUserName, AtfAccessToken) by its unique
        ///     identifier.
        /// </summary>
        /// <param name="repositoryId">Unique identifier of the repository record.</param>
        /// <param name="userConnection">
        ///     Optional user connection. If <c>null</c>, a new <see cref="UserConnection" /> is resolved via
        ///     <see cref="ClassFactory" />.
        /// </param>
        /// <returns>
        ///     <see cref="RepositoryInfo" />
        /// </returns>
        public static RepositoryInfo GetRepositoryInfo(Guid repositoryId, UserConnection userConnection = null) {
            IEntity repositoryEntity = FetchRepositoryEntityById(repositoryId, userConnection);
            if (repositoryEntity == null) {
                return null;
            }

            return new RepositoryInfo(
                repositoryEntity.GetTypedColumnValue<string>("AtfName"),
                repositoryEntity.GetTypedColumnValue<string>("AtfRepositoryUrl"),
                repositoryEntity.GetTypedColumnValue<string>("AtfUserName"),
                repositoryEntity.GetTypedColumnValue<string>("AtfAccessToken"),
				repositoryEntity.GetTypedColumnValue<string>("AtfActiveBranch")
			);
        }

        /// <summary>
        ///     Runs a process and returns the output and error messages.
        /// </summary>
        /// <param name="startInfo">The <see cref="ProcessStartInfo" /> to use.</param>
        /// <param name="waitForExit">If <c>true</c>, waits for the process to exit before returning.</param>
        /// <returns>The <see cref="OsProcessResult" />.</returns>
        public static OsProcessResult RunOsProcess(ProcessStartInfo startInfo, bool waitForExit = true) {
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            try {
                using (Process process = new Process()) {
                    process.StartInfo = startInfo;
                    if (waitForExit) {
                        process.OutputDataReceived += (sender, e) => {
                            if (!string.IsNullOrEmpty(e.Data)) {
                                output.AppendLine(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) => {
                            if (!string.IsNullOrEmpty(e.Data)) {
                                error.AppendLine(e.Data);
                            }
                        };
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                        return new OsProcessResult {
                            IsError = true, ErrorMessage = error.ToString(), Output = output.ToString()
                        };
                    }

                    process.Start();
                    return new OsProcessResult {
                        IsError = true
                    };
                }
            }
            catch (Exception ex) {
                return new OsProcessResult {
                    IsError = true, ErrorMessage = ex.Message, Output = output.ToString()
                };
            }
        }

        #endregion
    }

    #endregion

    #region Class: OsProcessResult

    public class OsProcessResult{
        #region Properties: Public

        /// <summary>
        ///     Indicates is a process ended with an Error
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        ///     Error message if any occurred
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Process output
        /// </summary>
        public string Output { get; set; }

        #endregion
    }

    #endregion

    #region Class: RepositoryInfo

    public class RepositoryInfo{
        #region Constructors: Public

        public RepositoryInfo(string name, string gitUrl, string userName, string password, string branchName) {
            Name = name;
            GitUrl = gitUrl;
            UserName = userName;
            Password = password;
            BranchName = branchName;
        }

        #endregion

        #region Properties: Public

        public string GitUrl { get; }
        public string Password { get; }
        public string UserName { get; }
        public string Name { get; }
        public string BranchName { get; }

        #endregion
    }

    #endregion

}
