using ConsoleGit;
using ConsoleGit.Commands;
using ErrorOr;

// args will have the following shape
// args[0] - command
// args[1] - gitUrl
// args[2] - userName
// args[3] - password
// args[4] - repoDir

CommandLineArgs consoleArgs;
ErrorOr<CommandLineArgs> consoleArgsOrError  = CommandLineArgs.Parse(args);
if(consoleArgsOrError.IsError){
	await Console.Error.WriteAsync($"{consoleArgsOrError.FirstError.Code} - {consoleArgsOrError.FirstError.Description}");
	return 1;
}
consoleArgs = consoleArgsOrError.Value;

ICommand command = consoleArgs.Command switch {
						"clone" => new CloneCommand(consoleArgs),
						"pull" => new CloneCommand(consoleArgs),
						"checkout" => new CheckoutCommand(consoleArgs),
						var _ => new ErrorCommand(consoleArgs)
					};

command.Execute().Match(
	success => OnSuccess(consoleArgs.Command, command),
	failure => OnFailure(consoleArgs.Command, failure, command)
);

return 0;



int OnFailure(string commandName, List<Error> errors, ICommand executedCommand){
	Console.Error.Write($"{commandName} command failed with error: {errors.FirstOrDefault().Code} - {errors.FirstOrDefault().Description}");
	executedCommand.Dispose();
	return 1;
}

int OnSuccess(string commandName,ICommand executedCommand){
	Console.Out.WriteLine($"{commandName} command executed successfully");
	executedCommand.Dispose();
	return 0;
}





//
//
//
// if(Directory.Exists(consoleArgs.RepoDir)){
// 	var directoryInfo = new DirectoryInfo(consoleArgs.RepoDir);
// 	GrantDeleteAccess(directoryInfo, Environment.UserName);
// 	directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ToList().ForEach(file => {
// 		file.Attributes = FileAttributes.Normal;
// 		file.Delete();
// 	});
// 	Directory.Delete(consoleArgs.RepoDir, true);
// 	Directory.CreateDirectory(consoleArgs.RepoDir);
// }
//
// using GitRepository repo = GitRepository.GetInstance(consoleArgs.UserName, consoleArgs.Password, consoleArgs.GitUrl, consoleArgs.RepoDir);
// 	
//
// repo.Clone().MatchFirst<Action>(
// 	success => ()=> Console.WriteLine(success),
// 	failure => ()=>Console.WriteLine($"{failure.Code} - {failure.Description}")
// ).Invoke();
//
// ErrorOr<IEnumerable<Branch>> isBranches = repo.ListLocalBranches();
// if (isBranches.IsError) {
// 	Console.WriteLine($"{isBranches.FirstError.Code} - {isBranches.FirstError.Description}");
// 	return;
// }
//
// foreach (Branch branch in isBranches.Value) {
// 	Console.WriteLine($"{branch.FriendlyName}");
//
// }
//
// ErrorOr<MergeResult?> isResult = repo.Pull();
// if (isResult.IsError) {
// 	Console.WriteLine($"{isResult.FirstError.Code} - {isResult.FirstError.Description}");
// }
//
// var x = repo.CheckoutBranch("origin/test");
// if (x.IsError) {
// 	Console.WriteLine($"{x.FirstError.Code} - {x.FirstError.Description}");
// }
//
// var commits = repo.GetCommits();
// Console.WriteLine("Commits: in origin/test");
// foreach (Commit commit in commits.Value) {
// 	Console.WriteLine($"{commit.Message}");
// }
//
//
// var xx = repo.CheckoutBranch("origin/master");
// if (x.IsError) {
// 	Console.WriteLine($"{x.FirstError.Code} - {x.FirstError.Description}");
// }
// var commitsinMaster = repo.GetCommits();
// Console.WriteLine("Commits: in origin/master");
// foreach (Commit commit in commitsinMaster.Value) {
// 	Console.WriteLine($"{commit.Message}");
// }
//
// repo.CreateBranchAndPublish("fromcs");
//
//
//
//
// return;
//
// static void GrantDeleteAccess(DirectoryInfo directoryInfo, string userName){
// 	// Get the current directory security settings
// 	DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
//
// 	// Define the delete access rule
// 	FileSystemAccessRule deleteRule = new FileSystemAccessRule(
// 		userName,
// 		FileSystemRights.Delete,
// 		InheritanceFlags.None,
// 		PropagationFlags.NoPropagateInherit,
// 		AccessControlType.Allow);
//
// 	// Add the rule
// 	directorySecurity.AddAccessRule(deleteRule);
//
// 	// Apply the updated security settings
// 	directoryInfo.SetAccessControl(directorySecurity);
// }