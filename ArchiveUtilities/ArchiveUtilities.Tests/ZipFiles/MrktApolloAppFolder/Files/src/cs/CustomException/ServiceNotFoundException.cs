using System;

namespace MrktApolloApp.CustomException
{
	/// <summary>
	/// Exception occurs when service cannot be resolved in DI container
	/// </summary>
	public class ServiceNotFoundException<T> : Exception
	{
		/// <summary>
		/// Creates instance of ServiceNotFoundException
		/// </summary>
		public ServiceNotFoundException() : base($"Service {typeof(T).FullName} not found"){ }

	}
}