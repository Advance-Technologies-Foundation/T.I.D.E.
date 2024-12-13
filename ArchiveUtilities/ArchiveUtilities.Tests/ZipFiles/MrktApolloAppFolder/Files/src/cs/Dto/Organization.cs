using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Organization
	{
		#region Properties: Public

		[JsonProperty("account")]
		public Account Account { get; set; }

		[JsonProperty("account_id")]
		public string AccountId { get; set; }

		[JsonProperty("alexa_ranking")]
		public int AlexaRanking { get; set; }

		[JsonProperty("angellist_url")]
		public string AngelListUrl { get; set; }

		[JsonProperty("annual_revenue")]
		public long AnnualRevenue { get; set; }

		[JsonProperty("annual_revenue_printed")]
		public string AnnualRevenuePrinted { get; set; }

		[JsonProperty("blog_url")]
		public string BlogUrl { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("crunchbase_url")]
		public string CrunchbaseUrl { get; set; }

		[JsonProperty("current_technologies")]
		public IEnumerable<CurrentTechnologies> CurrentTechnologies { get; set; }

		[JsonProperty("departmental_head_count")]
		public DepartmentalHeadCount DepartmentalHeadCount { get; set; }

		[JsonProperty("estimated_num_employees")]
		public int EstimatedNumEmployees { get; set; }

		[JsonProperty("facebook_url")]
		public string FacebookUrl { get; set; }

		[JsonProperty("founded_year")]
		public int FoundedYear { get; set; }

		[JsonProperty("funding_events")]
		public IEnumerable<FundingEvents> FundingEvents { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("industries")]
		public IEnumerable<string> Industries { get; set; }

		[JsonProperty("industry")]
		public string Industry { get; set; }

		[JsonProperty("industry_tag_hash")]
		public IndustryTagHash IndustryTagHash { get; set; }

		[JsonProperty("industry_tag_id")]
		public string IndustryTagId { get; set; }

		[JsonProperty("keywords")]
		public IEnumerable<string> Keywords { get; set; }

		[JsonProperty("languages")]
		public IEnumerable<string> Languages { get; set; }

		[JsonProperty("latest_funding_round_date")]
		public string LatestFundingRoundDate { get; set; }

		[JsonProperty("latest_funding_stage")]
		public string LatestFundingStage { get; set; }

		[JsonProperty("linkedin_uid")]
		public string LinkedinUid { get; set; }

		[JsonProperty("linkedin_url")]
		public string LinkedinUrl { get; set; }

		[JsonProperty("logo_url")]
		public string LogoUrl { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("num_suborganizations")]
		public int NumSuborganizations { get; set; }

		[JsonProperty("org_chart_root_people_ids")]
		public IEnumerable<string> OrgChartRootPeopleIds { get; set; }

		[JsonProperty("owned_by_organization_id")]
		public object OwnedByOrganizationId { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("postal_code")]
		public string PostalCode { get; set; }

		[JsonProperty("primary_domain")]
		public string PrimaryDomain { get; set; }

		[JsonProperty("primary_phone")]
		public PrimaryPhone PrimaryPhone { get; set; }

		[JsonProperty("publicly_traded_exchange")]
		public object PubliclyTradedExchange { get; set; }

		[JsonProperty("publicly_traded_symbol")]
		public object PubliclyTradedSymbol { get; set; }

		[JsonProperty("raw_address")]
		public string RawAddress { get; set; }

		[JsonProperty("retail_location_count")]
		public int RetailLocationCount { get; set; }

		[JsonProperty("sanitized_phone")]
		public string SanitizedPhone { get; set; }

		[JsonProperty("secondary_industries")]
		public IEnumerable<string> SecondaryIndustries { get; set; }

		[JsonProperty("seo_description")]
		public string SeoDescription { get; set; }

		[JsonProperty("short_description")]
		public string ShortDescription { get; set; }

		[JsonProperty("snippets_loaded")]
		public bool SnippetsLoaded { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("street_address")]
		public string StreetAddress { get; set; }

		[JsonProperty("suborganizations")]
		public object[] Suborganizations { get; set; }

		[JsonProperty("technology_names")]
		public IEnumerable<string> TechnologyNames { get; set; }

		[JsonProperty("total_funding")]
		public long TotalFunding { get; set; }

		[JsonProperty("total_funding_printed")]
		public string TotalFundingPrinted { get; set; }

		[JsonProperty("twitter_url")]
		public string TwitterUrl { get; set; }

		[JsonProperty("website_url")]
		public string WebsiteUrl { get; set; }

		#endregion
	}
}