using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Common.Logging;
using MrktApolloApp.Dto;
using MrktApolloApp.Dto.Converters;
using Newtonsoft.Json;

namespace MrktApolloApp.RestClient
{
	internal interface IApolloRestClient
	{

		#region Methods: Public

		/// <summary>
		/// This searches for organizations. Newer plans utilizing Export Credits will be deducting
		/// 1 export credit when calling this endpoint.
		/// </summary>
		/// <param name="searchTerm">Company name or website to search</param>
		/// <param name="includeExisting">Defining whether we want models prospected by current team or not.
		/// <c>false</c> means to look in net new database only, <c>true</c> means to see saved organizations only</param>
		/// <param name="page">Current page</param>
		/// <param name="take">Records per page</param>
		/// <returns></returns>
		/// <seealso href="https://apolloio.github.io/apollo-api-docs/?shell#organization-search">Organization search Docs</seealso>
		MixedCompanySearchResultDto SearchCompanyByTerm(string searchTerm, bool includeExisting, int page, int take);

		
		ResponseWrapper<PeopleSearchResponse> SearchPeople(HttpContent content);
		
		ResponseWrapper<PeopleMatchResponse> PeopleMatch(HttpContent content);
		#endregion

	}

	internal class ApolloRestClient : IApolloRestClient
	{


		#region Fields: Private

		private readonly ILog _logger;
		private readonly HttpClient _client;

		#endregion

		#region Constructors: Public

		public ApolloRestClient(IHttpClientFactory factory, ILog logger){
			_logger = logger;
			_client = factory.CreateClient("ApolloApi");
		}

		#endregion

		#region Methods: Private

		private static HttpContent CreateContent(BaseApolloRequest data){
			JsonSerializerSettings settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<JsonConverter> {
					new BoolToStringConverter(new[] {"prospected_by_current_team"})
				}
			};
			string dataString = JsonConvert.SerializeObject(data, settings);
			return new StringContent(dataString, Encoding.UTF8, "application/json");
		}

		#endregion

		#region Methods: Public

		public MixedCompanySearchResultDto SearchCompanyByTerm(string searchTerm, bool includeExisting, int page, int take){
			const string route = "/api/v1/mixed_companies/search";

			MixedCompanySearchDto data = new MixedCompanySearchDto(page, take) {
				SearchTerm = searchTerm,
				IncludeExisting = includeExisting
			};
			HttpResponseMessage result = _client.PostAsync(route, CreateContent(data)).GetAwaiter().GetResult();
			string responseString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			return JsonConvert.DeserializeObject<MixedCompanySearchResultDto>(responseString, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});
		}

		
		public ResponseWrapper<PeopleMatchResponse> PeopleMatch(HttpContent content){
			const string route = "/v1/people/match";
			HttpResponseMessage responseMessage = _client
				.PostAsync(route, content).GetAwaiter().GetResult();
			return new ResponseWrapper<PeopleMatchResponse>(responseMessage);
		}
		
		public ResponseWrapper<PeopleSearchResponse> SearchPeople(HttpContent content){
			const string route = "/v1/mixed_people/search";
			HttpResponseMessage responseMessage = _client
				.PostAsync(route, content).GetAwaiter().GetResult();
			return new ResponseWrapper<PeopleSearchResponse>(responseMessage);
		}
		
		#endregion
	}
}
