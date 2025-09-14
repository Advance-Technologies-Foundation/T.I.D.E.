using System.Text.Json.Serialization;

namespace AtfTIDE.GitBrowser.GitLab{

	public class NameSpace{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }
		
		[JsonPropertyName("kind")]
		public string Kind { get; set; }
		
		[JsonPropertyName("path")]
		public string Path { get; set; }
		
		[JsonPropertyName("full_path")]
		public string FullPath { get; set; }
		
		[JsonPropertyName("parent_id")]
		public int ParentId { get; set; }
	}

}
