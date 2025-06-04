using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace ConsoleGit.ConfiguredHttpClient;

public class LoginHandler :DelegatingHandler
{

	private readonly IOptions<CommandLineArgs> _args;
	private readonly CookieContainer _cookieContainer;
	private const string LoginRoute = "ServiceModel/AuthService.svc/Login";

	public LoginHandler(IOptions<CommandLineArgs> args, CookieContainer cookieContainer) {
		_args = args;
		_cookieContainer = cookieContainer;
	}
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken){
		
		Cookie? bpmcsrf = _cookieContainer
						.GetCookies(_args.Value.CreatioUrl!)
						.FirstOrDefault(c => c.Name == "BPMCSRF");
		
		if(bpmcsrf == null && !request.RequestUri!.ToString().EndsWith(LoginRoute)) {
			await LoginAsync();
		} 
		bpmcsrf = _cookieContainer
				.GetCookies(_args.Value.CreatioUrl!)
				.FirstOrDefault(c => c.Name == "BPMCSRF");
		
		if(bpmcsrf is not null && !request.RequestUri!.ToString().EndsWith(LoginRoute)) {
			request.Headers.Add("BPMCSRF", bpmcsrf.Value);
		}
		HttpResponseMessage result = await base.SendAsync(request, cancellationToken);
		if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized){
			Console.WriteLine("Unauthorized request, attempting to login...");
		}
		return result;
	}
	
	private async Task LoginAsync() {
		HttpRequestMessage requestMessage = _args.Value.IsFramework 
			? new HttpRequestMessage(HttpMethod.Post, _args.Value.CreatioUrl + "/0" + LoginRoute)
			: new HttpRequestMessage(HttpMethod.Post, _args.Value.CreatioUrl  +LoginRoute);
		requestMessage.Method = HttpMethod.Post;
		LoginRequest lr = new (_args.Value.CreatioUserName, _args.Value.CreatioPassword);
		requestMessage.Content = new StringContent(JsonSerializer.Serialize(lr), new System.Text.UTF8Encoding(), "application/json");
		HttpResponseMessage loginResult = await SendAsync(requestMessage, CancellationToken.None);
		if (!loginResult.IsSuccessStatusCode) {
			string errorMessage = await loginResult.Content.ReadAsStringAsync();
			throw new HttpRequestException($"Login failed with status code {loginResult.StatusCode}: {errorMessage}");
		}
		
	}
	
}

public record LoginRequest(
	[property:JsonPropertyName("UserName")] string UserName,
	[property:JsonPropertyName("UserPassword")] string UserPassword); 
