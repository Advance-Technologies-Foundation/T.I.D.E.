using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ErrorOr;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	public static class HelperFunctions {

		public static DirectoryInfo GetRepositoryDirectory(string repositoryName) {
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string repositoryDirectory = Path.Combine(baseDir, "conf","tide", repositoryName);
			return new DirectoryInfo(repositoryDirectory);
		}
		
		/// <summary>
		/// Gets the directory information for the Clio installation.
		/// </summary>
		/// <returns>A <see cref="DirectoryInfo"/> object representing the Clio directory.</returns>
		public static DirectoryInfo GetClioDirectory() {
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string clioDirectory = Path.Combine(baseDir, "conf","clio");
			return new DirectoryInfo(clioDirectory);
		}
		
		public static string GetClioFilePath() {
			DirectoryInfo cliodir = GetClioDirectory();
			return cliodir.GetFiles("clio.dll", SearchOption.AllDirectories).FirstOrDefault()?.FullName ?? string.Empty;
		}

		public static ErrorOr<FileInfo> GetConsoleGitPath(){
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string execFilePath = Path.Combine(baseDir, "Terrasoft.Configuration", "Pkg", "AtfTIDE", "Files",
												"exec", "ConsoleGit.exe");
			
			// Validate executable path exists
			if (!File.Exists(execFilePath)) {
				return Error.Failure("Process error", "Executable not found");
			}
			
			return new FileInfo(execFilePath);
			
		}
		
		public static string FetchRepositoryName(Guid repositoryId, UserConnection userConnection = null) {
			IEntity repositoryEntity = FetchRepositoryEntityById(repositoryId, userConnection);
			if(repositoryEntity == null) {
				return string.Empty;
			}
			string name = repositoryEntity.GetTypedColumnValue<string>("AtfName");
			return name ?? string.Empty;
		}
		
		
		public static RepositoryInfo GetRepositoryInfo (Guid repositoryId, UserConnection userConnection = null){
			IEntity repositoryEntity = FetchRepositoryEntityById(repositoryId, userConnection);
			if(repositoryEntity == null) {
				return null;
			}
			return new RepositoryInfo(
				repositoryEntity.GetTypedColumnValue<string>("AtfName"),
				repositoryEntity.GetTypedColumnValue<string>("AtfRepositoryUrl"),
				repositoryEntity.GetTypedColumnValue<string>("AtfUserName"),
				repositoryEntity.GetTypedColumnValue<string>("AtfAccessToken")
			);
		}
		
		private static IEntity FetchRepositoryEntityById(Guid repositoryId, UserConnection userConnection = null){
			UserConnection uc = userConnection ?? ClassFactory.Get<UserConnection>();
			const string repositorySchemaName = "AtfRepository";
			Entity repositoryEntity = uc.EntitySchemaManager
				.GetInstanceByName(repositorySchemaName)
				.CreateEntity(uc);
			bool isFetched = repositoryEntity.FetchFromDB(repositoryId);
			return !isFetched ? null : repositoryEntity;
		}
		
		public static Dictionary<Guid, Dictionary<string,string>> ClioArguments = new Dictionary<Guid, Dictionary<string,string>>();
		
		public static void AddClioArgsForUser(Guid userId, Dictionary<string,string> args) {
			if(ClioArguments.ContainsKey(userId)) {
				ClioArguments[userId] = args;
			}else {
				ClioArguments.Add(userId, args);
			}
		}
		
		
		public static OsProcessResult RunOsProcess(ProcessStartInfo startInfo, bool waitForExit = true) {
			StringBuilder output = new StringBuilder();
			StringBuilder error = new StringBuilder();
			try {
				using (Process process = new Process()) {
					process.StartInfo = startInfo;
					if(waitForExit) {
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
						return new OsProcessResult() {
							IsError = true,
							ErrorMessage = error.ToString(),
							Output = output.ToString()
						};
					}
					process.Start();
					return new OsProcessResult() {
						IsError = true,
					};
				}
			} catch (Exception ex) {
				return new OsProcessResult() {
					IsError = true,
					ErrorMessage = ex.Message,
					Output = output.ToString()
				}; 
			}
		}
		
		public static DirectoryInfo CreateTempDirectory(){
			DirectoryInfo tempDir = Directory.CreateDirectory(Path.Combine(GetClioDirectory().FullName, ".dotnet_tmp"));
			if(!tempDir.Exists) {
				tempDir.Create();
			}
			return tempDir;
		}
	}
	
	
	
	
	public class RepositoryInfo {

		public RepositoryInfo(string name, string gitUrl, string userName, string password) {
			Name = name;
			GitUrl = gitUrl;
			UserName = userName;
			Password = password;
			
		}
		public string GitUrl { get; }
		public string Password { get; }
		public string UserName { get; }
		public string Name { get; }
	}
	
	public class OsProcessResult {

		public bool IsError { get; set; }
		public string ErrorMessage { get; set; }
		public string Output { get; set; }

	}
	
}
