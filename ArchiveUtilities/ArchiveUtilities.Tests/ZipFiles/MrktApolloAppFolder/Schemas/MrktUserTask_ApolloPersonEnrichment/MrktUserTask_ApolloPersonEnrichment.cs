using System.Diagnostics.CodeAnalysis;
using MrktApolloApp;
using MrktApolloApp.DomainModels;

namespace Terrasoft.Core.Process.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;

	#region Class: MrktUserTask_ApolloPersonEnrichment

	/// <exclude/>
	/// <summary>
	/// Partial class MrktUserTask_ApolloPersonEnrichment.
	/// This class is responsible for enriching a person's data using the Apollo application.
	/// It contains methods for executing the enrichment process and creating a composite object from the enriched data.
	/// </summary>
	public partial class MrktUserTask_ApolloPersonEnrichment
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			EnrichedPerson data = ApolloApplication.Instance.EnrichPerson(Contact);
			CompositeObject compositeObject = CreateCompositeObject(data);
			EnrichedPerson = new CompositeObjectList<CompositeObject> {
				compositeObject
			};
			
			if(!string.IsNullOrWhiteSpace(data.ErrorMessage)) {
				Success = false;
				ErrorMessage = data.ErrorMessage;
				return true;
			}
			
			if(string.IsNullOrWhiteSpace(data.ApolloId)) {
				Success = false;
				ErrorMessage = "No matching data was found in Apollo.io using the Contact information provided for this record. Please check the accuracy of the Full name, Account, and Email.";
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
		
		/// <summary>
		/// Creates a composite object from an EnrichedPerson object.
		/// This method maps the properties of the EnrichedPerson object to key-value pairs in the CompositeObject.
		/// The keys are the property names and the values are the property values.
		/// </summary>
		/// <param name="enrichedPerson">The EnrichedPerson object to be mapped.</param>
		/// <returns>A CompositeObject that contains the mapped data.</returns>
		private static CompositeObject CreateCompositeObject(EnrichedPerson enrichedPerson) =>
			new CompositeObject {
				new KeyValuePair<string, object>("ApolloId",enrichedPerson.ApolloId),
				new KeyValuePair<string, object>("City",enrichedPerson.City),
				new KeyValuePair<string, object>("Country",enrichedPerson.Country),
				new KeyValuePair<string, object>("FacebookUrl",enrichedPerson.FacebookUrl),
				new KeyValuePair<string, object>("FirstName",enrichedPerson.FirstName),
				new KeyValuePair<string, object>("LastEmploymentStartDate",enrichedPerson.LastEmploymentStartDate),
				new KeyValuePair<string, object>("LastEmploymentTitle",enrichedPerson.LastEmploymentTitle),
				new KeyValuePair<string, object>("LastName",enrichedPerson.LastName),
				new KeyValuePair<string, object>("LinkedinUrl",enrichedPerson.LinkedinUrl),
				new KeyValuePair<string, object>("OrganizationId",enrichedPerson.OrganizationId),
				new KeyValuePair<string, object>("Phone",enrichedPerson.Phone),
				new KeyValuePair<string, object>("State",enrichedPerson.Region),
				new KeyValuePair<string, object>("Title",enrichedPerson.Title),
				new KeyValuePair<string, object>("TwitterUrl",enrichedPerson.TwitterUrl),
				new KeyValuePair<string, object>("Email",enrichedPerson.Email),
				new KeyValuePair<string, object>("PersonalEmail",enrichedPerson.PersonalEmail),
			};
	}

	#endregion

}

