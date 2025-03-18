using System.Threading.Tasks;

namespace AtfTIDE.HttpClient {
	
	public interface IGitLabHttpClient {

		Task<string> GetGoogle();

	}
	
	public class GitLabHttpClient : IGitLabHttpClient{

		private readonly System.Net.Http.HttpClient _httpClient;

		public GitLabHttpClient(System.Net.Http.HttpClient httpClient){
			_httpClient = httpClient;
		}
		
		public async Task<string> GetGoogle(){
			
			var response = await _httpClient.GetAsync("https://www.google.com");
			if(response.IsSuccessStatusCode){
				return await response.Content.ReadAsStringAsync();
			}
			else{
				throw new System.Exception("Failed to get Google");
			}
			
		}
	}
}
