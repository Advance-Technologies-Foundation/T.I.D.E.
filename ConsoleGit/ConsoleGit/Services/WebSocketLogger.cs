using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace ConsoleGit.Services;

public interface IWebSocketLogger {

	Task LogAsync(MessageType messageType, string message);

}

public class WebSocketLogger : IWebSocketLogger {

	private readonly IOptions<CommandLineArgs> _args;
	private readonly HttpClient _client;
	private const string WebSocketEndpoint = "rest/CreatioApiGateway/SendEventToUI";
	public WebSocketLogger(IHttpClientFactory factory, IOptions<CommandLineArgs> args) {
		_args = args;
		_client = factory.CreateClient("initializedClient");
	}
	private Uri GetRouteUri() {
		return _args.Value.IsFramework 
			? new Uri("0/"+WebSocketEndpoint, UriKind.Relative)
			: new Uri(WebSocketEndpoint, UriKind.Relative);
	}
	public async Task LogAsync(MessageType messageType, string message) {
		HttpRequestMessage requestMessage = new () {
			RequestUri = GetRouteUri(),
			Method = HttpMethod.Post,
		};
		EventModel eventModel = new (messageType, message);
		requestMessage.Content = new StringContent(eventModel.ToJson(), new System.Text.UTF8Encoding(), "application/json");
		var result = await _client.SendAsync(requestMessage);
		
		var msg = await result.Content.ReadAsStringAsync();
		if (!result.IsSuccessStatusCode) {
			await Console.Out.WriteLineAsync($"Failed to log message: {messageType} - {message} due to:{msg}");
		}
	}
}

public enum MessageType {
		INF,
		ERR,
		WAR
	}

public record EventModel {

	[JsonPropertyName("Sender")]
	public string Sender { get; init; }
		
	[JsonPropertyName("Content")]
	public string Content { get; init; }

	[JsonPropertyName("CommandName")]
	public string CommandName { get; init; }

	public EventModel(MessageType messageType, string message) {
		Sender = "ConsoleGit";
		CommandName = "LogMessage";
		Content = $"[{messageType.ToString().ToUpper()}] - {message}";
	}
	public string ToJson() {
		return JsonSerializer.Serialize(this);
	}

}

