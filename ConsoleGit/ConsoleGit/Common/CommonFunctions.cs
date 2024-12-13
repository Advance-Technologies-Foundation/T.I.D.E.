using ErrorOr;

namespace ConsoleGit.Common;

public static class CommonFunctions {

	#region Methods: Public

	/// <summary>
	///  Executes an action with retry logic, attempting the operation multiple times before giving up.
	/// </summary>
	/// <param name="action">The action to execute.</param>
	/// <param name="retryCount">The maximum number of retry attempts. Defaults to 3.</param>
	/// <param name="delay">The time to wait between retry attempts. If not specified, defaults to 1 second.</param>
	/// <returns>
	///  A Success result if the action succeeds, or a Failure with error code "RETRY_COUNT_EXCEEDED"
	///  if all retry attempts fail.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when action is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when retryCount is less than or equal to 0.</exception>
	public static ErrorOr<Success> Retry(Action action, int retryCount = 3, TimeSpan? delay = null){
		ArgumentNullException.ThrowIfNull(action);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(retryCount);
		TimeSpan retryDelay = delay ?? TimeSpan.FromSeconds(1);
		if (delay < TimeSpan.Zero) {
			throw new ArgumentOutOfRangeException(nameof(delay));
		}

		for (int attempt = 0; attempt < retryCount; attempt++) {
			try {
				action();
				return Result.Success;
			} catch {
				Thread.Sleep(retryDelay); // Wait for 1 second before retrying
			}
		}
		return Error.Failure("RETRY_COUNT_EXCEEDED", "Retry count exceeded");
	}

	/// <summary>
	///  Executes an asynchronous action with retry logic, attempting the operation multiple times before giving up.
	/// </summary>
	/// <param name="action">The asynchronous action to execute.</param>
	/// <param name="retryCount">The maximum number of retry attempts. Defaults to 3.</param>
	/// <param name="delay">The time to wait between retry attempts. If not specified, defaults to 1 second.</param>
	/// <returns>
	///  A Task containing a Success result if the action succeeds, or a Failure with error code
	///  "RETRY_COUNT_EXCEEDED" if all retry attempts fail.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown when action is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when retryCount is less than or equal to 0.</exception>
	public static async Task<ErrorOr<Success>>
		RetryAsync(Func<Task> action, int retryCount = 3, TimeSpan? delay = null){
		ArgumentNullException.ThrowIfNull(action);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(retryCount);
		TimeSpan retryDelay = delay ?? TimeSpan.FromSeconds(1);
		if (delay < TimeSpan.Zero) {
			throw new ArgumentOutOfRangeException(nameof(delay));
		}

		for (int attempt = 0; attempt < retryCount; attempt++) {
			try {
				await action();
				return Result.Success;
			} catch {
				await Task.Delay(retryDelay);
			}
		}
		return Error.Failure("RETRY_COUNT_EXCEEDED", "Retry count exceeded");
	}

	#endregion

}