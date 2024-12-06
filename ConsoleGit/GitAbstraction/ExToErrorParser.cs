using System;
using ErrorOr;
using LibGit2Sharp;

namespace GitAbstraction {
	public static class ExToErrorParser {

		#region Methods: Private

		private static Error OnNameConflictException(NameConflictException ex){
			return Error.Failure(ex.GetType().Name, ex.Message);
		}

		private static Error OnRecurseSubmodulesException(RecurseSubmodulesException ex){
			return Error.Failure(ex.GetType().Name, ex.Message);
		}

		private static Error OnRepositoryNotFoundException(RepositoryNotFoundException ex){
			return Error.Failure(ex.GetType().Name, ex.Message);
		}

		private static Error OnUserCancelledException(UserCancelledException ex){
			return Error.Failure(ex.GetType().Name, ex.Message);
		}

		#endregion

		#region Methods: Public

		public static Error ToError(this Exception e){
			switch (e) {
				case RecurseSubmodulesException ex:
					return OnRecurseSubmodulesException(ex);
				case UserCancelledException ex:
					return OnUserCancelledException(ex);
				case NameConflictException ex:
					return OnNameConflictException(ex);
				case RepositoryNotFoundException ex:
					return OnRepositoryNotFoundException(ex);
				case var _:
					return Error.Failure(e.Message);
			}
		}

		#endregion

	}
}