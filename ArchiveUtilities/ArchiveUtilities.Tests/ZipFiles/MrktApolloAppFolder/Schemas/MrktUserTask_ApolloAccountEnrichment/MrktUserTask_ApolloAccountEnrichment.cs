using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MrktApolloApp;
using MrktApolloApp.DomainModels;
using Terrasoft.Common;

namespace Terrasoft.Core.Process.Configuration
{


	#region Class: MrktUserTask_ApolloAccountEnrichment

	/// <exclude/>
	public partial class MrktUserTask_ApolloAccountEnrichment
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			EnrichedAccount data = ApolloApplication.Instance.EnrichAccount(Domain);
			CompositeObject compositeObject = CreateCompositeObject(data);
			MrktEnrichedAccount = new CompositeObjectList<CompositeObject> {
				compositeObject
			};
			
			if(!string.IsNullOrWhiteSpace(data.ErrorMessage)) {
				Success = false;
				ErrorMessage = data.ErrorMessage;
				return true;
			}
			
			if(string.IsNullOrWhiteSpace(data.ApolloId)) {
				Success = false;
				ErrorMessage = "No matching data was found in Apollo.io using the Web provided for this account. Please double-check the Web details for accuracy.";
				return true;
			}
			
			Success = true;
			return true;
		}

		#endregion

		#region Methods: Public

		
		[ExcludeFromCodeCoverage]
		public override string GetExecutionData() {
			return string.Empty;
		}

		#endregion

		private CompositeObject CreateCompositeObject(EnrichedAccount data) =>
			new CompositeObject {
				new KeyValuePair<string, object>("ApolloId",data.ApolloId),
				new KeyValuePair<string, object>("Name",data.Name),
				new KeyValuePair<string, object>("WebsiteUrl",data.WebSiteUrl),
				new KeyValuePair<string, object>("BlogUrl",data.BlogUrl),
				new KeyValuePair<string, object>("AngellistUrl",data.AngellistUrl),
				new KeyValuePair<string, object>("LinkedinUrl",data.LinkedinUrl),
				new KeyValuePair<string, object>("TwitterUrl",data.TwitterUrl),
				new KeyValuePair<string, object>("FacebookUrl",data.FacebookUrl),
				new KeyValuePair<string, object>("Phone",data.Phone),
				new KeyValuePair<string, object>("LogoUrl",data.LogoUrl),
				new KeyValuePair<string, object>("Description",data.Description),
				new KeyValuePair<string, object>("AccountAnnualRevenue",data.AccountAnnualRevenue),
				new KeyValuePair<string, object>("AccountEmployeesNumber",data.AccountEmployeeNumber),
				new KeyValuePair<string, object>("AccountIndustry",data.AccountIndustry),
				new KeyValuePair<string, object>("StreetAddress",data.StreetAddress),
				new KeyValuePair<string, object>("City",data.City),
				new KeyValuePair<string, object>("State",data.Region),
				new KeyValuePair<string, object>("Country",data.Country),
				new KeyValuePair<string, object>("PostalCode",data.PostalCode),
				new KeyValuePair<string, object>("Domain",data.Domain),
			};
	}

	#endregion

}

