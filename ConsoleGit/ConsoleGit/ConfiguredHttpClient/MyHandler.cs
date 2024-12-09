namespace ConsoleGit.ConfiguredHttpClient;

public class MyHandler :DelegatingHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken){

#if DEBUG
		Console.WriteLine($"Sending request to {request.RequestUri}");
		var headers = request.Headers;
		foreach (var header in headers){
			Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
		}
#endif
		
		return base.SendAsync(request, cancellationToken);
	}

	

}