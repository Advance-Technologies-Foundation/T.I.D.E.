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

	internal class Rule3 : RuleBase
	{

		#region Constructors: Public

		public Rule3(IApolloRestClient restClient, IDataProvider dataProvider, bool isActive,IWebSocket webSocket)
			: base(restClient, dataProvider, isActive, webSocket){ }

		#endregion

		#region Methods: Public

		protected override EnrichedPerson InternalExecute(Guid contactId){
			Contact contact = AppDataContextFactory
				.GetAppDataContext(DataProvider)
				.GetModel<Contact>(contactId);

			string firstName = contact?.GivenName ?? string.Empty;
			string lastName = contact?.Surname ?? string.Empty;
			string web = contact?.Account?.Web ?? string.Empty;

			if (string.IsNullOrWhiteSpace(web)) {
				return Next(contactId);
			}

			if (!web.ToLowerInvariant().StartsWith("http://") && !web.ToLowerInvariant().StartsWith("https://")) {
				web = "https://" + web;
			} 
			
			bool isValidUrl = Uri.TryCreate(web, UriKind.RelativeOrAbsolute, out Uri uri);
			if (!isValidUrl) {
				return Next(contactId);
			}
			uri.GetLeftPart( UriPartial.Authority );
			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName)) {
				Next(contactId);
			} 
			MatchRequestDto contentObj = new MatchRequestDto {
				FirstName = firstName,
				LastName = lastName,
				OrganizationDomain = uri.GetLeftPart(UriPartial.Authority)
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
			return string.IsNullOrWhiteSpace(mappedPerson.Email) ? Next(contactId) : mappedPerson;
		}

		#endregion

	}
}