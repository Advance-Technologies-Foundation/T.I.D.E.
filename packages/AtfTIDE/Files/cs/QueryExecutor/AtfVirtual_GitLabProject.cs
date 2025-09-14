using System;
using System.Collections.Generic;
using System.Linq;
using ATF.Repository;
using ATF.Repository.Providers;
using AtfTIDE.GitBrowser;
using AtfTIDE.GitBrowser.GitLab;
using AtfTIDE.RepositoryModels;
using Common.Logging;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace AtfTIDE.QueryExecutor{

	[DefaultBinding(typeof(IEntityQueryExecutor), Name = "AtfVirtual_GitLabProjectQueryExecutor")]
	public class AtfVirtual_GitLabProjectQueryExecutor : IEntityQueryExecutor{

		private readonly ILog _logger;
		private readonly UserConnection _userConnection;
		private const string SchemaName = "AtfVirtual_GitLabProject";

		public AtfVirtual_GitLabProjectQueryExecutor() {
			_userConnection = ClassFactory.Get<UserConnection>();
			_logger = LogManager.GetLogger(TideConsts.LoggerName);
		}
		
		private IEnumerable<string> GetNameSpaceFromEsq(IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			string titleFilterValue = esqFilterParser.GetEsqFilterValueByKey("NameSpace", esq);
			return string.IsNullOrWhiteSpace(titleFilterValue)
				? Array.Empty<string>()
				: titleFilterValue.Split(',');
		}
		private IEnumerable<string> GetNameFromEsq(IEsqFilterParser esqFilterParser, EntitySchemaQuery esq){
			string titleFilterValue = esqFilterParser.GetEsqFilterValueByKey("Name", esq);
			return string.IsNullOrWhiteSpace(titleFilterValue)
				? Array.Empty<string>()
				: titleFilterValue.Split(',');
		}
		
		public EntityCollection GetEntityCollection(EntitySchemaQuery esq) {
			
			EntitySchema schema = esq.RootSchema;
			EntityCollection collection = new EntityCollection(_userConnection, schema);


			IEsqFilterParser esqFilterParser = TideApp.Instance.GetRequiredService<IEsqFilterParser>();
			string keywordsFilterValue = esqFilterParser.GetSearchBoxFilterValue(esq);
			IEnumerable<string> nameSpaceFilter = GetNameSpaceFromEsq(esqFilterParser, esq);
			IEnumerable<string> repositoryNameFilter = GetNameFromEsq(esqFilterParser, esq);

			var defaultGitServer = FindDefaultGitServer(_userConnection);
			// if (defaultGitServer.Url) {
			// 	
			// }
			//
			// IGitlabProvider x = TideApp.Instance.GetRequiredService<IGitlabProvider>();
			// x.Configure(null, "")
			// 	.GetAllRepositoriesAsync();
			
			
			
			List<ProjectDto> projects = new List<ProjectDto> {
				new ProjectDto {
					ProjectId = 1150,
					Name = "Apollo",
					Description =  "Enhance accuracy and quality of <b>Accounts</b> and <b>Contacts</b> in Creatio by data enrichment from \r\n<a href=\"https://www.apollo.io\" target=\"_blank\">Apollo.io</a>",
					Namespace = new NameSpace{
						Id = 385, 
						Name = "Marketplace",
						Kind = "group",
						FullPath = "marketplace",
						Path = "marketplace",
					},
					CreatedOn = DateTime.Parse("2023-11-27T08:21:12.719+02:00"),
					ModifiedOn = DateTime.Parse("2025-09-14T13:30:52.131+03:00"),
					CloneUrl = new Uri("https://tscore-git.creatio.com/marketplace/apollo.git"),
				}, 
				new ProjectDto {
					ProjectId = 1561,
					Name = "BGK-Bank",
					Description =  "Sample repository demonstrating various development approaches, and CI/CD pipeline",
					Namespace = new NameSpace{
						Id = 2047, 
						Name = "Solution Architecture",
						Kind = "group",
						FullPath = "solution-architecture",
						Path = "solution-architecture",
					},
					CreatedOn = DateTime.Parse("2025-09-05T20:31:17.045+03:00"),
					ModifiedOn = DateTime.Parse("2025-09-14T12:28:18.576+03:00"),
					CloneUrl = new Uri("https://gitlab.com/atf-tide/atf-tide.git"),
				}
			};

			foreach (ProjectDto project in projects) {
				Entity entity = schema.CreateEntity(_userConnection);
				FillEntity(entity, project);
				collection.Add(entity);
			}
			return collection;
		}
		
		private void FillEntity(Entity entity, ProjectDto project) {
			entity.SetColumnValue("CreatedOn", project.CreatedOn);
			entity.SetColumnValue("ModifiedOn", project.ModifiedOn);
			EntitySchemaColumn createdByColumn = entity.Schema.Columns.FindByName("CreatedBy");
			entity.SetColumnBothValues(createdByColumn, _userConnection.CurrentUser.ContactId.ToString(), _userConnection.CurrentUser.ContactName);
			
			EntitySchemaColumn modifiedByColumn = entity.Schema.Columns.FindByName("ModifiedBy");
			entity.SetColumnBothValues(modifiedByColumn, _userConnection.CurrentUser.ContactId.ToString(), _userConnection.CurrentUser.ContactName);
			
			EntitySchemaColumn gColumn = entity.Schema.Columns.FindByName("AtfGitServer");
			AtfGitServer defaultGitServer = FindDefaultGitServer(_userConnection);
			entity.SetColumnBothValues(gColumn, defaultGitServer.Id, defaultGitServer.Name);
			
			entity.SetColumnValue("ProjectId", project.ProjectId);
			entity.SetColumnValue("Name", project.Name);
			entity.SetColumnValue("Description", project.Description);
			entity.SetColumnValue("NameSpace", project.Namespace?.FullPath ?? string.Empty);
			entity.SetColumnValue("CloneUrl", project.CloneUrl);
		}

		private AtfGitServer FindDefaultGitServer(UserConnection userConnection) {
			IDataProvider provider  = new LocalDataProvider(userConnection);
			IAppDataContext ctx = ATF.Repository.AppDataContextFactory.GetAppDataContext(provider);
			return ctx
				.Models<AtfGitServer>()
				.FirstOrDefault(i => i.Default == true);
		}
	}

}

