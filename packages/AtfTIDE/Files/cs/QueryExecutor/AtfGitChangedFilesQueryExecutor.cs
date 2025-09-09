using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using AtfTIDE.Logging;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AtfTIDE.QueryExecutor {
	
	[DefaultBinding(typeof(IEntityQueryExecutor), Name = "AtfGitChangedFilesQueryExecutor")]
	public class AtfGitChangedFilesQueryExecutor : IEntityQueryExecutor {
		public AtfGitChangedFilesQueryExecutor() { }
		
		public EntityCollection GetEntityCollection(EntitySchemaQuery esq){
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			Guid repositoryId = GetAtfRepositoryIdFromFilters(esq.Filters);
			RepositoryInfo repositoryInfo = HelperFunctions.GetRepositoryInfo(repositoryId, userConnection);
			
			ConsoleGitArgs args = new ConsoleGitArgs {
				Command = AtfTIDE.Commands.GetChangedFiles,
				GitUrl = repositoryInfo.GitUrl,
				Password = repositoryInfo.Password,
				UserName = repositoryInfo.UserName,
				RepoDir = HelperFunctions.GetRepositoryDirectory(repositoryInfo.Name).ToString(),
			};
			
			ConsoleGitResult gitCommandResult = ClassFactory
												.Get<IConsoleGit>("AtfTIDE.ConsoleGit")
												.Execute(args);
			
			EntityCollection collection = new EntityCollection(userConnection, esq.RootSchema);
			if(gitCommandResult.ExitCode != 0) {
				string errorMessage = $"{gitCommandResult.Output} {gitCommandResult.ErrorMessage}"; 
				TideApp.Instance.GetRequiredService<ILiveLogger>().LogError(errorMessage);
				return collection;
			}

			string cleanContent = gitCommandResult.Output
				.Replace("GetChangedFiles command executed successfully", "")
				.Trim();
			
			List<ChangedFileDto> files = JsonSerializer.Deserialize<List<ChangedFileDto>>(cleanContent);

			foreach (ChangedFileDto file in files) {
				Entity entity = esq.RootSchema.CreateEntity(userConnection);
				entity.SetDefColumnValues();
				entity.SetColumnValue("AtfFileName", file.Path);
				entity.SetColumnValue("AtfRepositoryId", repositoryId);
				entity.SetColumnValue("AtfFileStatus", file.Status);
				collection.Add(entity);
			}
			return collection;
		}
		
		

		private static readonly Func<EntitySchemaQueryFilterCollection, Guid>
			GetAtfRepositoryIdFromFilters = filters => {
				Guid atfRepository = Guid.Empty;
				filters
					.Where(f => f is EntitySchemaQueryFilter && f.IsEnabled)
					.ForEach(f => {
						_ = f.GetFilterInstances()
							.Where(i => i != null && i.IsEnabled &&
								i.LeftExpression.ExpressionType == EntitySchemaQueryExpressionType.SchemaColumn &&
								i.LeftExpression.Path == "[AtfRepository:Id:Id].Id" && i.RightExpressions.Count == 1)
							.Select(b => b.RightExpressions[0].ParameterValue)
							.FirstOrDefault(p => Guid.TryParse(p?.ToString(), out atfRepository));
					});
				return atfRepository;
			};
	}
	
	public class ChangedFileDto {

		[JsonPropertyName("Path")]
		public string Path { get; set; }
	
		[JsonPropertyName("Status")]
		public string Status { get; set; }

	}
	
	
}