using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATF.Repository;
using ATF.Repository.Providers;
using AtfTIDE.GitBrowser;
using AtfTIDE.GitBrowser.GitLab;
using AtfTIDE.Logging;
using AtfTIDE.RepositoryModels;
using Common.Logging;
using ErrorOr;
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
		private readonly ILiveLogger _liveLogger;

		public AtfVirtual_GitLabProjectQueryExecutor() {
			_userConnection = ClassFactory.Get<UserConnection>();
			_logger = LogManager.GetLogger(TideConsts.LoggerName);
			_liveLogger = TideApp.Instance.GetRequiredService<ILiveLogger>();
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

			AtfGitServer defaultGitServer = FindDefaultGitServer(_userConnection);

			IEnumerable<ProjectDto> projects = new List<ProjectDto>();
			
			if (defaultGitServer.GitRepositoryTypeId == Guid.Parse("b1e6ed87-2bf2-4efd-acad-ce45bc17371d")) {
				IGitlabProvider x = TideApp.Instance.GetRequiredService<IGitlabProvider>();
				Uri.TryCreate(defaultGitServer.Url, UriKind.Absolute, out Uri gitlabUrl);
				Task.Run(async () => {
					IConfiguredProvider confProvider = x.Configure(gitlabUrl, defaultGitServer.AccessToken);
					ErrorOr<IEnumerable<ProjectDto>> isErrorOrProjects = await confProvider.GetAllRepositoriesByFilterAsync(keywordsFilterValue ?? string.Empty, esq.RowCount, esq.SkipRowCount);
					
					isErrorOrProjects.Match<object>(
						onValue: value => { 
							projects = isErrorOrProjects.Value;
							return null; 
						},
						onError: errors => {
							errors.ForEach(e => {
								_logger.ErrorFormat("{Code} - {Description}",
									isErrorOrProjects.FirstError.Code,
									isErrorOrProjects.FirstError.Description);
								
								_liveLogger.LogError($"{isErrorOrProjects.FirstError.Code} - {isErrorOrProjects.FirstError.Description}");
							});
							return null;
						}
					);
					
				}).GetAwaiter().GetResult();
			}
			foreach (ProjectDto project in projects) {
				Entity entity = schema.CreateEntity(_userConnection);
				FillEntity(entity, project, defaultGitServer);
				collection.Add(entity);
			}
			return collection;
		}
		
		private void FillEntity(Entity entity, ProjectDto project, AtfGitServer defaultGitServer) {
			entity.SetColumnValue("CreatedOn", project.CreatedOn);
			entity.SetColumnValue("ModifiedOn", project.ModifiedOn);
			EntitySchemaColumn createdByColumn = entity.Schema.Columns.FindByName("CreatedBy");
			entity.SetColumnBothValues(createdByColumn, _userConnection.CurrentUser.ContactId.ToString(), _userConnection.CurrentUser.ContactName);
			
			EntitySchemaColumn modifiedByColumn = entity.Schema.Columns.FindByName("ModifiedBy");
			entity.SetColumnBothValues(modifiedByColumn, _userConnection.CurrentUser.ContactId.ToString(), _userConnection.CurrentUser.ContactName);
			
			EntitySchemaColumn gColumn = entity.Schema.Columns.FindByName("AtfGitServer");
			entity.SetColumnBothValues(gColumn, defaultGitServer.Id, defaultGitServer.Name);
			
			entity.SetColumnValue("Id", Guid.NewGuid());
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

