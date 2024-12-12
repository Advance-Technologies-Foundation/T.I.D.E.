using System;

namespace MrktApolloApp.DomainModels
{
	
	/// <summary>
	/// Represents enriched account data
	/// </summary>
	/// <remarks>
	/// All members initialized to it default values.
	/// </remarks>
	public class EnrichedAccount
	{
		/// <value>
		/// The <c>ApolloId</c> property represents a unique identifier of an organization
		/// </value>
		public string ApolloId { get; set; } = string.Empty;
		
		/// <value>
		/// Organization Name
		/// </value>
		public string Name { get; set; } = string.Empty;
		
		/// <value>
		/// Main website of an organization
		/// </value>
		public string WebSiteUrl { get; set; } = string.Empty;
		
		/// <value>
		/// Blog url of an organization
		/// </value>
		public string BlogUrl { get; set; } = string.Empty;
		public string AngellistUrl { get; set; } = string.Empty;
		public string LinkedinUrl { get; set; } = string.Empty;
		public string TwitterUrl { get; set; } = string.Empty;
		public string FacebookUrl { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string LogoUrl { get; set; } = string.Empty;
		
		/// <value>
		/// Short description of an organization
		/// </value>
		public string Description { get; set; } = string.Empty;
		public Guid AccountAnnualRevenue { get; set; } = Guid.Empty;
		public Guid AccountEmployeeNumber { get; set; } = Guid.Empty;
		
		/// <value>
		/// Value from Creatio Account industry mapped to Appolo industry
		/// </value>
		public Guid AccountIndustry { get; set; } = Guid.Empty;
		
		/// <value>
		/// Street address
		/// </value>
		public string StreetAddress { get; set; } = string.Empty;
		
		/// <value>
		/// Id of a City found in <see cref="MrktApolloApp.Repository.Models.City"/>
		/// </value>
		public Guid City { get; set; } = Guid.Empty;

		/// <value>
		/// Id of a Region found in <see cref="MrktApolloApp.Repository.Models.Region"/>
		/// </value>
		public Guid Region { get; set; } = Guid.Empty;
		
		/// <value>
		/// Id of a Country found in <see cref="MrktApolloApp.Repository.Models.Country"/>
		/// </value>
		public Guid Country { get; set; } = Guid.Empty;

		/// <value>
		/// Postal Code of an organization,
		/// for the US postal code is XXXXX-XXXX digits, for Canada it is XXX-XXX digits
		/// </value>
		public string PostalCode { get; set; } = string.Empty;

		/// <value>
		/// Primary domain of an organization, used for enrichment
		/// </value>
		public string Domain { get; set; } = string.Empty;
		
		public string ErrorMessage{get;set;}
		
	}
}