namespace Terrasoft.Core.Process
{

	using AtfTIDE.cs.GitBrowser;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: AtfFindAppRepositoriesOnServerMethodsWrapper

	/// <exclude/>
	public class AtfFindAppRepositoriesOnServerMethodsWrapper : ProcessModel
	{

		public AtfFindAppRepositoriesOnServerMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var gitServerEntityName = "AtfGitServer";
			var gitServerEntitySchema = UserConnection.EntitySchemaManager.GetInstanceByName(gitServerEntityName);
			var gitServerEntity = gitServerEntitySchema.CreateEntity(UserConnection);
			if (gitServerEntity.FetchFromDB(Get<Guid>("AtfGitServer"))) {
			 
			    string gitUrl = gitServerEntity.GetTypedColumnValue<string>("Url");
			    string gitlabToken = gitServerEntity.GetTypedColumnValue<string>("AccessToken");;
			    string userName = gitServerEntity.GetTypedColumnValue<string>("UserName");;
			   
			    var gitlabBrowser = new GitLabBrowser(gitUrl);
			    var repositories = gitlabBrowser.FindClioRepositoriesOnServer(gitlabToken);
			    foreach (var repo in repositories) {
			        var entitySchemaName = "AtfRepository";
			            var entitySchema = UserConnection.EntitySchemaManager.GetInstanceByName(entitySchemaName);
			            var entity = entitySchema.CreateEntity(UserConnection);
			            entity.SetDefColumnValues();
			            entity.SetColumnValue("AtfName", repo.Name);
			            entity.SetColumnValue("AtfAccessToken", gitlabToken);
			            entity.SetColumnValue("AtfRepositoryUrl", repo.UrlToClone);
			            entity.SetColumnValue("AtfUserName", userName);
			           
			            if (!entity.Save()) {
			                throw new Exception("Could not save record!");
			            }
			    }
			}
			 
			return true;
		}

		#endregion

	}

	#endregion

}

