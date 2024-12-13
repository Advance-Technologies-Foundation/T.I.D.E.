using System;
using System.Collections.Generic;
using System.IO;
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
	
}