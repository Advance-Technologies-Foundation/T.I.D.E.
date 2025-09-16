using System;
using System.Collections.Generic;
using ATF.Repository;
using ATF.Repository.Attributes;

namespace AtfTIDE.RepositoryModels{

	[Schema("AtfGitServer")]
	public class AtfGitServer: BaseModel{
		
		[SchemaProperty("Name")]
		public string Name { get; set; }

		[SchemaProperty("Description")]
		public string Description { get; set; }

		[SchemaProperty("AccessToken")]
		public string AccessToken { get; set; }

		[SchemaProperty("UserName")]
		public string UserName { get; set; }

		[SchemaProperty("Url")]
		public string Url { get; set; }

		[SchemaProperty("Code")]
		public string Code { get; set; }

		[SchemaProperty("Default")]
		public bool Default { get; set; }

		/// <summary>
		/// Provider for git (Gitlab, GitHub, BitBucket, etc)
		/// </summary>
		[SchemaProperty("GitRepositoryType")]
		public Guid GitRepositoryTypeId { get; set; }

		/// <inheritdoc cref="GitRepositoryTypeId"/>
		[LookupProperty("GitRepositoryType")]
		public virtual AtfGitRepositoryType GitRepositoryType { get; set; }
		
	}

	[Schema("AtfGitRepositoryType")]
	public class AtfGitRepositoryType : BaseModel{

		[SchemaProperty("Name")]
		public string Name { get; set; }

		[SchemaProperty("Description")]
		public string Description { get; set; }
		
		[DetailProperty(nameof(AtfGitServer))]
		public virtual List<AtfGitServer> Servers { get; set; }
		
	}

}
