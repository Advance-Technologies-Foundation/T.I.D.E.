using ATF.Repository;
using ATF.Repository.Attributes;

namespace AtfTIDE.RepositoryModels{

	[Schema("AtfGitServer")]
	public class AtfGitServer: BaseModel{

		[SchemaProperty("Name")]
		public string Name { get; set; }
		
		[SchemaProperty("Default")]
		public bool Default { get; set; }
		
		[SchemaProperty("AccessToken")]
		public string AccessToken { get; set; }
		
		[SchemaProperty("Code")]
		public string Code { get; set; }
		
		[SchemaProperty("Url")]
		public string Url { get; set; }
		
		[SchemaProperty("UserName")]
		public string UserName { get; set; }
		
	}

}
