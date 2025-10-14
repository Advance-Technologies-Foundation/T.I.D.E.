using System.Net;
using ConsoleGit.Commands;
using ConsoleGit.ConfiguredHttpClient;
using ConsoleGit.Services;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace ConsoleGit;

public static class Program
{
	public static async Task<int> Main(string[] args) {
		
		IHostBuilder builder = Host.CreateDefaultBuilder(args)
									.ConfigureAppConfiguration(config => {
										config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
									})
									.ConfigureLogging((context, logging) => {
										logging.ClearProviders();
										logging.AddConfiguration(context.Configuration.GetSection("Logging"));
										logging.AddConsole();
										logging.SetMinimumLevel(LogLevel.Warning);
									})
									.ConfigureServices(services => {
										services.AddSingleton<CommandLineArgs>();
				
										services
											.AddHttpClient("initializedClient")
											.ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler { 
												CookieContainer = sp.GetRequiredService<CookieContainer>() 
											})
											.ConfigureHttpClient((sp, client) => {
												CommandLineArgs cla = sp.GetRequiredService<CommandLineArgs>();
												client.BaseAddress = cla.CreatioUrl;
						
											})
											.AddHttpMessageHandler<LoginHandler>()
#if DEBUG			
											.AddHttpMessageHandler<MyHandler>()
#endif
											.AddPolicyHandler(
												HttpPolicyExtensions
													.HandleTransientHttpError()
													.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
													.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
											);
#if DEBUG
										services.AddScoped<MyHandler>();
#endif
										services.AddScoped<LoginHandler>();
										services.AddSingleton<CookieContainer>();
										services.AddScoped<DownloadPackagesCommand>();
										services.AddScoped<CloneCommand>();
										services.AddScoped<PullCommand>();
										services.AddScoped<CheckoutCommand>();
										services.AddScoped<PushCommand>();
										services.AddScoped<AddAllCommand>();
										services.AddScoped<CommitCommand>();
										services.AddScoped<DownloadPackagesCommand>();
										services.AddScoped<GetActiveBranchCommand>();
										services.AddScoped<GetBranchesCommand>();
										services.AddScoped<GetDiffCommand>();
										services.AddScoped<GetChangedFilesCommand>();
										services.AddScoped<DiscardFilesCommand>();
										services.AddTransient<IWebSocketLogger, WebSocketLogger>();
										// Register other commands
									});

		using IHost host = builder.Build();
		CommandLineArgs consoleArgs = host.Services.GetRequiredService<CommandLineArgs>();

		
		IWebSocketLogger logger = host.Services.GetRequiredService<IWebSocketLogger>();
		await logger.LogAsync(MessageType.INF, $"ConsoleGit started with command: {consoleArgs.Command}");
		
		using ICommand command = consoleArgs.Command switch {
									"Clone" => host.Services.GetRequiredService<CloneCommand>(),
									"Pull" => host.Services.GetRequiredService<PullCommand>(),
									"Checkout" => host.Services.GetRequiredService<CheckoutCommand>(),
									"Push" => host.Services.GetRequiredService<PushCommand>(),
									"AddAll" =>host.Services.GetRequiredService<AddAllCommand>(),
									"Commit" =>host.Services.GetRequiredService<CommitCommand>(),
									"DownloadPackages" =>host.Services.GetRequiredService<DownloadPackagesCommand>(),
									"GetActiveBranch" => host.Services.GetRequiredService<GetActiveBranchCommand>(),
									"GetBranches" =>host.Services.GetRequiredService<GetBranchesCommand>(),
									"GetDiff" =>host.Services.GetRequiredService<GetDiffCommand>(),
									"GetChangedFiles" =>host.Services.GetRequiredService<GetChangedFilesCommand>(),
									"DiscardFiles" =>host.Services.GetRequiredService<DiscardFilesCommand>(),
									var _ => new ErrorCommand(consoleArgs)
								};
		
		
		
		return await command.Execute().MatchAsync(
			_ => consoleArgs.Silent ? Task.FromResult(0): OnSuccess(consoleArgs.Command),
			failure => OnFailure(consoleArgs.Command, failure)
		);
		
		async Task<int> OnFailure(string commandName, List<Error> errors){
			foreach (Error error in errors) {
				await host.Services.GetRequiredService<IWebSocketLogger>()
					.LogAsync(MessageType.ERR, $"CONSOLE_GIT: {commandName} command failed with error: {error.Code} - {error.Description}");
			}
			return 1;
		}
		
		async Task<int> OnSuccess(string commandName){
			await host.Services.GetRequiredService<IWebSocketLogger>()
					.LogAsync(MessageType.INF, $"CONSOLE_GIT: {commandName} command executed successfully");
			return 0;
		}
	}
}
