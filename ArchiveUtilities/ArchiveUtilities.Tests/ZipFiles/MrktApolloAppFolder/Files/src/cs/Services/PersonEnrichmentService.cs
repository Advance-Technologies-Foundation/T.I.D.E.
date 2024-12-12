using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using ATF.Repository;
using ATF.Repository.Providers;
using Common.Logging;
using MrktApolloApp.Chain;
using MrktApolloApp.DomainModels;
using MrktApolloApp.Dto;
using MrktApolloApp.RestClient;
using Newtonsoft.Json;
using MrktApolloSearchRules = MrktApolloApp.Repository.Models.MrktApolloSearchRules;

namespace MrktApolloApp.Services
{
	internal interface IPersonEnrichmentService
	{

		#region Methods: Public

		EnrichedPerson GetEnrichedData(Guid contactId);

		IEnumerable<Person> SearchedPeopleInOrganization(string organizationId, int skip, int take);
		(IEnumerable<Person> people, IEnumerable<Contact> contacts) SearchedPeopleAndContactsInOrganization(
			SearchPeopleRequestDto dto);

		#endregion

	}

	internal class PersonEnrichmentService : IPersonEnrichmentService
	{

		#region Fields: Private

		private readonly IApolloRestClient _restClient;
		private readonly ILog _logger;
		private readonly IDataProvider _dataProvider;
		private readonly IWebSocket _webSocket;

		private readonly JsonSerializerSettings _defaultSerializerSettings = new JsonSerializerSettings {
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.None
		};

		#endregion

		#region Constructors: Public

		public PersonEnrichmentService(IApolloRestClient restClient, ILog logger, IDataProvider dataProvider, IWebSocket webSocket){
			_restClient = restClient;
			_logger = logger;
			_dataProvider = dataProvider;
			_webSocket = webSocket;
		}

		#endregion

		#region Methods: Public

		public EnrichedPerson GetEnrichedData(Guid contactId){
			if (contactId == Guid.Empty) {
				return new EnrichedPerson();
			}
			List<MrktApolloSearchRules> activeRules = AppDataContextFactory
				.GetAppDataContext(_dataProvider)
				.Models<MrktApolloSearchRules>()
				.Where(m => m.MrktIsActive)
				.OrderBy(rule => rule.Name)
				.ToList();
			if (!activeRules.Any()) {
				return new EnrichedPerson();
			}

			Rule1 rule1 = new Rule1(_restClient, _dataProvider,
				activeRules.FirstOrDefault(r => r.Name == nameof(Rule1))?.MrktIsActive ?? false, _webSocket);
			Rule2 rule2 = new Rule2(_restClient, _dataProvider,
				activeRules.FirstOrDefault(r => r.Name == nameof(Rule2))?.MrktIsActive ?? false, _webSocket);
			Rule3 rule3 = new Rule3(_restClient, _dataProvider,
				activeRules.FirstOrDefault(r => r.Name == nameof(Rule3))?.MrktIsActive ?? false, _webSocket);
			rule1.SetNext(rule2);
			rule2.SetNext(rule3);
			return rule1.Execute(contactId);
		}

		[ExcludeFromCodeCoverage]
		public IEnumerable<Person> SearchedPeopleInOrganization(string organizationId, int skip, int take){
			SearchPeopleRequestDto contentObj = new SearchPeopleRequestDto {
				PageLimit = take,
				PageNumber = 1 + skip / take,
				OrganizationIds = new[] {organizationId}
			};
			string json = JsonConvert.SerializeObject(contentObj, _defaultSerializerSettings);
			HttpContent content = new StringContent(json);
			ResponseWrapper<PeopleSearchResponse> response = _restClient.SearchPeople(content);
			PeopleSearchResponse a = response.Data;
			return a?.People ?? Array.Empty<Person>();
		}
		
		[ExcludeFromCodeCoverage]
		public (IEnumerable<Person> people, IEnumerable<Contact> contacts) SearchedPeopleAndContactsInOrganization(
			SearchPeopleRequestDto dto){
			string json = JsonConvert.SerializeObject(dto, _defaultSerializerSettings);
			HttpContent content = new StringContent(json);
			ResponseWrapper<PeopleSearchResponse> response = _restClient.SearchPeople(content);
			
			if(!string.IsNullOrWhiteSpace(response.ErrorMessage)) {
				_webSocket.PostMessage(
					"SearchedPeopleAndContactsInOrganization", 
					"ShowSnackBarMessage", 
					"MrktApolloSelectContact_MiniPage", Guid.Empty, response.ErrorMessage);
				_logger.ErrorFormat("Error while SearchedPeopleAndContactsInOrganization: ",response.ErrorMessage);
				return (Array.Empty<Person>(), Array.Empty<Contact>());
			}
			
			PeopleSearchResponse a = response.Data;
			return (a?.People ?? Array.Empty<Person>(), a?.Contacts ?? Array.Empty<Contact>());
		}
		#endregion

	}
}