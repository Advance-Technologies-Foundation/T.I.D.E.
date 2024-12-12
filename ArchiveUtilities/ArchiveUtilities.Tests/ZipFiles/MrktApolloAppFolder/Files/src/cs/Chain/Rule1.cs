using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using ATF.Repository;
using ATF.Repository.Providers;
using MrktApolloApp.DomainModels;
using MrktApolloApp.Dto;
using MrktApolloApp.RestClient;
using MrktApolloApp.Services;
using Newtonsoft.Json;
using Contact = MrktApolloApp.Repository.Models.Contact;

namespace MrktApolloApp.Chain
{
	
	internal class Rule1 : RuleBase
	{

		#region Constructors: Public

		public Rule1(IApolloRestClient restClient, IDataProvider dataProvider, bool isActive, IWebSocket webSocket)
			: base(restClient, dataProvider, isActive, webSocket){ }

		#endregion

		#region Methods: Public

		protected override EnrichedPerson InternalExecute(Guid contactId){
			string apolloId = AppDataContextFactory
				.GetAppDataContext(DataProvider)
				.GetModel<Contact>(contactId)?.MrktApolloId ?? string.Empty;

			if (string.IsNullOrEmpty(apolloId)) {
				return Next(contactId);
			}

			MatchRequestDto contentObj = new MatchRequestDto {
				Id = apolloId
			};

			string json = JsonConvert.SerializeObject(contentObj, DefaultSerializerSettings);
			HttpContent content = new StringContent(json);
			ResponseWrapper<PeopleMatchResponse> responseObj = RestClient.PeopleMatch(content);
			if(!string.IsNullOrWhiteSpace(responseObj.ErrorMessage)) {
				SendMessage(_webSocket, contactId, responseObj.ErrorMessage);
				return new EnrichedPerson {
					ErrorMessage = responseObj.ErrorMessage
				};
			}
			
			if(!ContainsData(responseObj))
			{
				return Next(contactId);
			}
			
			EnrichedPerson mappedPerson = MapPerson(responseObj.Data.Person, DataProvider);
			return string.IsNullOrWhiteSpace(mappedPerson.FirstName) ? Next(contactId) : mappedPerson;
			
		}

		#endregion

	}
}