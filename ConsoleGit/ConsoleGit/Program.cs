using ConsoleGit;
using ConsoleGit.Commands;
using ErrorOr;

// args will have the following shape
// args[0] - command
// args[1] - gitUrl
// args[2] - userName
// args[3] - password
// args[4] - repoDir

ErrorOr<CommandLineArgs> consoleArgsOrError  = CommandLineArgs.Parse(args);
if(consoleArgsOrError.IsError){
	await Console.Error.WriteAsync($"{consoleArgsOrError.FirstError.Code} - {consoleArgsOrError.FirstError.Description}");
	return 1;
}
CommandLineArgs consoleArgs = consoleArgsOrError.Value;
using ICommand command = consoleArgs.Command switch {
						"clone" => new CloneCommand(consoleArgs),
						"pull" => new PullCommand(consoleArgs),
						"checkout" => new CheckoutCommand(consoleArgs),
						"push" => new PushCommand(consoleArgs),
						"add" =>new AddCommand(consoleArgs),
						"commit" =>new CommitCommand(consoleArgs),
						var _ => new ErrorCommand(consoleArgs)
					};

return await command.Execute().MatchAsync(
	_ => OnSuccess(consoleArgs.Command),
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