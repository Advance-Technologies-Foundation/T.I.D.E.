using System;
using System.Collections.Generic;
using System.Linq;
using ATF.Repository;
using ATF.Repository.Providers;
using MrktApolloApp.DomainModels;
using MrktApolloApp.Dto;
using MrktApolloApp.RestClient;
using MrktApolloApp.Services;
using Newtonsoft.Json;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using City = MrktApolloApp.Repository.Models.City;
using Country = MrktApolloApp.Repository.Models.Country;
using Region = MrktApolloApp.Repository.Models.Region;

namespace MrktApolloApp.Chain
{
	/// <summary>
	/// An interface for Chain Item in the Chain of Responsibility pattern implementation.
	/// </summary>
	/// <typeparam name="T">The type of the object that this chain item works on.</typeparam>
	internal interface IChainItem<out T> where T : class, new()
	{

		#region Methods: Public

		/// <summary>
		/// Executes the chain item functionality for the provided contact id.
		/// </summary>
		/// <param name="contactId">Identifier of the contact.</param>
		/// <returns>An object of type T.</returns>
		T Execute(Guid contactId);

		/// <summary>
		/// Returns the next chain element for the provided contact id.
		/// </summary>
		/// <param name="contactId">Identifier of the contact.</param>
		/// <returns>An object of type T from the next element in the chain.</returns>
		T Next(Guid contactId);

		/// <summary>
		/// Sets the next chain element in the chain.
		/// </summary>
		/// <param name="item">The next item in the chain.</param>
		void SetNext(IChainItem<EnrichedPerson> item);

		#endregion

	}

	/// <summary>
	/// The base class for all rules in the chain of responsibility pattern implementation.
	/// </summary>
	/// <remarks>
	/// This class provides the basic functionality for a rule in the chain, including the ability to execute the rule,
	/// move to the next rule in the chain, and set the next rule in the chain. It also provides a method for executing
	/// the specific functionality of the rule, which must be implemented by derived classes.
	/// </remarks>
	internal abstract class RuleBase : IChainItem<EnrichedPerson>
	{

		#region Fields: Private

		private static readonly Func<IDataProvider, string, City> GetCityByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<City>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IDataProvider, string, Country> GetCountryByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<Country>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IDataProvider, string, Region> GetRegionByName
			= (dataProvider, name) => AppDataContextFactory.GetAppDataContext(dataProvider)
				.Models<Region>()
				.FirstOrDefault(c => c.Name == name);

		private static readonly Func<IEnumerable<EmploymentHistory>, (string Title, DateTime StartDate)>
			GetLatestEmploymentHistory = eh => eh
				.OrderByDescending(e => e.StartDate)
				.Select(e => (e.Title, StartDate: e.StartDate.Date))
				.FirstOrDefault();

		
		protected Action<IWebSocket, Guid, string> SendMessage = (webSocket, recordId, message)=>{
			webSocket.PostMessage(
				"RuleChain", 
				"ShowSnackBarMessage", 
				"Contacts_FormPage", recordId, message,ClassFactory.Get<UserConnection>());
		};
		
		private readonly bool _isActive;
		protected readonly IWebSocket _webSocket;

		#endregion

		#region Fields: Protected

		protected readonly IApolloRestClient RestClient;
		protected readonly IDataProvider DataProvider;

		#endregion

		#region Constructors: Protected

		/// <summary>
		/// Initializes a new instance of the RuleBase class.
		/// </summary>
		/// <param name="restClient">Client for making REST requests.</param>
		/// <param name="dataProvider">Provider of data access operations.</param>
		/// <param name="isActive">Flag indicating whether this rule is active or not.</param>
		protected RuleBase(IApolloRestClient restClient, IDataProvider dataProvider, bool isActive, IWebSocket webSocket){
			RestClient = restClient;
			DataProvider = dataProvider;
			_isActive = isActive;
			_webSocket = webSocket;
		}

		#endregion

		#region Properties: Private

		private IChainItem<EnrichedPerson> NextItem { get; set; }

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Executes the specific functionality of this rule for the given contact id.
		/// </summary>
		/// <param name="contactId">Identifier of the contact for which this rule should be applied.</param>
		/// <returns>The enriched person object with data processed by this rule.</returns>
		protected abstract EnrichedPerson InternalExecute(Guid contactId);

		#endregion

		#region Methods: Public

		/// <summary>
		/// Executes this rule for the given contact id.
		/// </summary>
		/// <param name="contactId">Identifier of the contact for which this rule should be applied.</param>
		/// <returns>If this rule is active, returns the enriched person object with data processed by this rule, otherwise moves to the next rule in the chain.</returns>
		public EnrichedPerson Execute(Guid contactId){
			return !_isActive
				? Next(contactId)
				: InternalExecute(contactId);
		}

		/// <summary>
		/// Returns the result from the next rule in the chain for the given contact id.
		/// </summary>
		/// <param name="contactId">Identifier of the contact for which the next rule should be applied.</param>
		/// <returns>The enriched person object with data processed by the next rule in the chain.</returns>
		public virtual EnrichedPerson Next(Guid contactId){
			return NextItem == null
				? new EnrichedPerson()
				: NextItem.Execute(contactId);
		}

		/// <summary>
		/// Sets the next rule in the chain.
		/// </summary>
		/// <param name="item">The next rule in the chain.</param>
		public virtual void SetNext(IChainItem<EnrichedPerson> item) => NextItem = item;

		#endregion

		private protected readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings {
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.None
		};

		private protected static EnrichedPerson MapPerson(Person person, IDataProvider dataProvider){
			(string Title, DateTime StartDate) emp = GetLatestEmploymentHistory(person.EmploymentHistory);
			EnrichedPerson enrichedPerson = new EnrichedPerson {
				ApolloId = person.Id,
				FacebookUrl = person.Contact?.FacebookUrl ?? person.FacebookUrl ?? string.Empty,
				LinkedinUrl = person.Contact?.LinkedinUrl ?? person.LinkedinUrl ?? string.Empty,
				TwitterUrl = person.Contact?.TwitterUrl ?? person.TwitterUrl ?? string.Empty,
				FirstName = person.Contact?.FirstName ?? person.FirstName ?? string.Empty,
				LastName = person.Contact?.LastName ?? person.LastName ?? string.Empty,
				OrganizationId = person.OrganizationId ?? string.Empty,
				Phone = person.Contact?.SanitizedPhone ??
					person.PhoneNumbers?.FirstOrDefault()?.SanitizedNumber ?? string.Empty,
				Title = person.Contact?.Title ?? person.Title ?? string.Empty,
				City = GetCityByName(dataProvider, person.City)?.Id ?? Guid.Empty,
				Region = GetRegionByName(dataProvider, person.State)?.Id ?? Guid.Empty,
				Country = GetCountryByName(dataProvider, person.Country)?.Id ?? Guid.Empty,
				Email = person.Contact?.Email ?? person.Email ?? string.Empty,
				PersonalEmail = person.PersonalEmails?.FirstOrDefault() ?? string.Empty,
				LastEmploymentTitle = emp.Title ?? string.Empty,
				LastEmploymentStartDate = emp.StartDate
			};
			return enrichedPerson;
		}

		private protected bool ContainsData(ResponseWrapper<PeopleMatchResponse> responseObj){
			if (!string.IsNullOrWhiteSpace(responseObj?.ErrorMessage)) {
				return false;
			}
			return responseObj?.Data?.Person != null;
		}

	}
}