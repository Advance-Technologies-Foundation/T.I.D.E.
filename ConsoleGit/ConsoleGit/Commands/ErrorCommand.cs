using System.Globalization;
using ErrorOr;

namespace ConsoleGit.Commands;

public sealed class ErrorCommand(CommandLineArgs args) : BaseRepositoryCommand(args) {

	private const string ErrorCode = "INVALID_COMMAND";
	private const string DescriptionPattern = "Invalid command name: {0}.";
	public override ErrorOr<Success> Execute() => Error.Failure(ErrorCode, 
			string.Format(CultureInfo.InvariantCulture, DescriptionPattern, Args.Command));
}
