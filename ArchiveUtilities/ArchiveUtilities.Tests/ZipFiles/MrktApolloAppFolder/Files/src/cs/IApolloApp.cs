using System;
using MrktApolloApp.CustomException;
using Terrasoft.Common;
using Terrasoft.Core;
using MrktApolloApp.DomainModels;

namespace MrktApolloApp
{
	/// <summary>
	/// Main application interface.
	/// This is the entry point for all Apollo services.
	/// </summary>
	/// <example>
	/// <code>
	/// IApolloApp client = AppStartUp.ApolloApp;
	/// </code>
	/// </example>
	public interface IApolloApp : IDisposable
	{
		/// <summary>
		/// Service locator for Apollo services.
		/// </summary>
		/// <typeparam name="T">Type of requested service</typeparam>
		/// <returns>Instance of the service</returns>
		/// <exception cref="ServiceNotFoundException{T}">Thrown when service cannot be resolved in DI container </exception>  
		/// <example>
		/// To get an instance of a service of type T, use the following code:
		/// <code>
		/// IApolloRestClient client = AppStartUp.ApolloApp.GetService&lt;IApolloRestClient&gt;();
		/// </code>
		/// </example>
		/// <seealso cref="ServiceNotFoundException{T}"/>
		T GetService<T>();

		/// <summary>
		/// Acquires enriched data for the given domain.
		/// </summary>
		/// <param name="domain">Domain to use for enrichment</param>
		/// <returns>Enriched Data</returns>
		/// <seealso cref="EnrichedAccount"/>
		EnrichedAccount EnrichAccount(string domain);
		
		
		/// <summary>
		/// Posts websocket message to all users. Used to invoke client side event handlers.
		/// </summary>
		/// <param name="senderName"></param>
		/// <param name="commandName"></param>
		/// <param name="schemaName"></param>
		/// <param name="recordId"></param>
		/// <param name="message"></param>
		/// <param name="userConnection">Optional parameter</param>
		void PostWebSocketMessage(string senderName, string commandName, string schemaName, Guid recordId, 
			string message, UserConnection userConnection = null);

		
		/// <summary>
		/// Enriches person data by contact id.
		/// </summary>
		/// <param name="contactId"></param>
		/// <returns></returns>
		EnrichedPerson EnrichPerson(Guid contactId);
		
		
	}
}