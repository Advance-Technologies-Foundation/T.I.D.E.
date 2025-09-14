using System;
using System.Text.Json.Serialization;

namespace AtfTIDE.GitBrowser.GitLab{

	public class ProjectDto{
		[JsonPropertyName("id")]
		public int ProjectId { get; set; }
	
		[JsonPropertyName("description")]
		public string Description { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }
		
		[JsonPropertyName("namespace")]
		public NameSpace Namespace { get; set; }
		
		[JsonPropertyName("created_at")]
		public DateTime CreatedOn { get; set; }

		[JsonPropertyName("last_activity_at")]
		public DateTime ModifiedOn { get; set; }
		
		[JsonPropertyName("created_at")]
		public Uri CloneUrl { get; set; }
		
	}

}
