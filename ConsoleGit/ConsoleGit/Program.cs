using System.Net;using System.Text;
using ConsoleGit;
using ConsoleGit.Commands;
using ConsoleGit.ConfiguredHttpClient;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration(config => {
		config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
	})
	.ConfigureLogging((context, logging) => {
		logging.ClearProviders();
		logging.AddConfiguration(context.Configuration.GetSection("Logging"));
		logging.AddConsole();
	})
	.ConfigureServices(services => {
		services.AddSingleton<CommandLineArgs>();
		services.AddHttpClient("initializedClient")
			.ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler { 
				CookieContainer = sp.GetRequiredService<CommandLineArgs>().CreateCookieContainerFromArgs() 
			})
			.ConfigureHttpClient((sp, client) => {
				CommandLineArgs args = sp.GetRequiredService<CommandLineArgs>();
				client.BaseAddress = args.CreatioUrl;
				
#if DEBUG				
				Console.WriteLine($"Downloading packages from {args.CreatioUrl}");
#endif
				CookieContainer cookieContainer = args.CreateCookieContainerFromArgs();
				const string bpmcsrf = "BPMCSRF";
				string bpmcsrfValue = cookieContainer.GetCookies(args.CreatioUrl!)
										.FirstOrDefault(c => c.Name == bpmcsrf)?.Value ?? "";
				client.DefaultRequestHeaders.Add(bpmcsrf, bpmcsrfValue);
			})
#if DEBUG			
			.AddHttpMessageHandler<MyHandler>()
#endif
			.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
								.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
								.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
			);
#if DEBUG
		services.AddScoped<MyHandler>();
#endif
		services.AddScoped<DownloadPackagesCommand>();
		services.AddScoped<CloneCommand>();
		services.AddScoped<PullCommand>();
		services.AddScoped<CheckoutCommand>();
		services.AddScoped<PushCommand>();
		services.AddScoped<AddAllCommand>();
		services.AddScoped<CommitCommand>();
		services.AddScoped<DownloadPackagesCommand>();
		services.AddScoped<GetBranchesCommand>();
		services.AddScoped<GetDiffCommand>();
		services.AddScoped<GetChangedFilesCommand>();
		// Register other commands
	});

using IHost host = builder.Build();
CommandLineArgs consoleArgs = host.Services.GetRequiredService<CommandLineArgs>();

using ICommand command = consoleArgs.Command switch {
	"Clone" => host.Services.GetRequiredService<CloneCommand>(),
	"Pull" => host.Services.GetRequiredService<PullCommand>(),
	"Checkout" => host.Services.GetRequiredService<CheckoutCommand>(),
	"Push" => host.Services.GetRequiredService<PushCommand>(),
	"AddAll" =>host.Services.GetRequiredService<AddAllCommand>(),
	"Commit" =>host.Services.GetRequiredService<CommitCommand>(),
	"DownloadPackages" =>host.Services.GetRequiredService<DownloadPackagesCommand>(),
	"GetBranches" =>host.Services.GetRequiredService<GetBranchesCommand>(),
	"GetDiff" =>host.Services.GetRequiredService<GetDiffCommand>(),
	"GetChangedFiles" =>host.Services.GetRequiredService<GetChangedFilesCommand>(),
	var _ => new ErrorCommand(consoleArgs)
};


return await command.Execute().MatchAsync(
	_ => consoleArgs.Silent ? Task.FromResult(0): OnSuccess(consoleArgs.Command),
	failure => OnFailure(consoleArgs.Command, failure)
);

async Task<int> OnFailure(string commandName, List<Error> errors){
	await Console.Error.WriteAsync($"{commandName} command failed with error: {errors.FirstOrDefault().Code} - {errors.FirstOrDefault().Description}");
	return 1;
}

async Task<int> OnSuccess(string commandName){
	await Console.Out.WriteLineAsync($"{commandName} command executed successfully");
	return 0;
}

