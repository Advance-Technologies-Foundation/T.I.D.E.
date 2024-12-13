using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Company
	{

		#region Properties: Public

		[JsonProperty("account_playbook_statuses")]
		public object[] AccountPlaybookStatuses { get; set; }

		[JsonProperty("account_rule_config_statuses")]
		public IEnumerable<object> AccountRuleConfigStatuses { get; set; }

		[JsonProperty("account_stage_id")]
		public string AccountStageId { get; set; }

		[JsonProperty("alexa_ranking")]
		public int AlexaRanking { get; set; }

		[JsonProperty("angellist_url")]
		public string AngellistUrl { get; set; }

		[JsonProperty("blog_url")]
		public object BlogUrl { get; set; }

		[JsonProperty("campaign_status_tally")]
		public ContactCampaignStatusTally ContactCampaignStatusTally { get; set; }

		[JsonProperty("contact_emailer_campaign_ids")]
		public IEnumerable<object> ContactEmailerCampaignIds { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("creator_id")]
		public string CreatorId { get; set; }

		[JsonProperty("crm_owner_id")]
		public object CrmOwnerId { get; set; }

		[JsonProperty("crunchbase_url")]
		public object CrunchbaseUrl { get; set; }

		[JsonProperty("domain")]
		public string Domain { get; set; }
		
		[JsonProperty("existence_level")]
		public string ExistenceLevel { get; set; }

		[JsonProperty("facebook_url")]
		public string FacebookUrl { get; set; }

		[JsonProperty("founded_year")]
		public int FoundedYear { get; set; }

		[JsonProperty("hubspot_id")]
		public object HubspotId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("intent_strength")]
		public object IntentStrength { get; set; }

		[JsonProperty("label_ids")]
		public object[] LabelIds { get; set; }

		[JsonProperty("languages")]
		public IEnumerable<string> Languages { get; set; }

		[JsonProperty("last_activity_date")]
		public DateTime LastActivityDate { get; set; }

		[JsonProperty("linkedin_uid")]
		public string LinkedinUid { get; set; }

		[JsonProperty("linkedin_url")]
		public string LinkedinUrl { get; set; }

		[JsonProperty("logo_url")]
		public string LogoUrl { get; set; }
		
		[JsonProperty("modality")]
		public string Modality { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("num_contacts")]
		public int NumContacts { get; set; }

		[JsonProperty("organization_city")]
		public string OrganizationCity { get; set; }

		[JsonProperty("organization_country")]
		public string OrganizationCountry { get; set; }

		[JsonProperty("organization_id")]
		public string OrganizationId { get; set; }

		[JsonProperty("organization_postal_code")]
		public string OrganizationPostalCode { get; set; }

		[JsonProperty("organization_raw_address")]
		public string OrganizationRawAddress { get; set; }

		[JsonProperty("organization_state")]
		public string OrganizationState { get; set; }

		[JsonProperty("organization_street_address")]
		public string OrganizationStreetAddress { get; set; }

		[JsonProperty("original_source")]
		public string OriginalSource { get; set; }

		[JsonProperty("owned_by_organization_id")]
		public object OwnedByOrganizationId { get; set; }

		[JsonProperty("owner_id")]
		public string OwnerId { get; set; }

		[JsonProperty("parent_account_id")]
		public object ParentAccountId { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("phone_status")]
		public string PhoneStatus { get; set; }

		[JsonProperty("primary_domain")]
		public string PrimaryDomain { get; set; }

		[JsonProperty("primary_phone")]
		public PrimaryPhone PrimaryPhone { get; set; }

		[JsonProperty("publicly_traded_exchange")]
		public object PubliclyTradedExchange { get; set; }

		[JsonProperty("publicly_traded_symbol")]
		public object PubliclyTradedSymbol { get; set; }

		[JsonProperty("salesforce_id")]
		public object SalesforceId { get; set; }

		[JsonProperty("sanitized_phone")]
		public string SanitizedPhone { get; set; }

		[JsonProperty("show_intent")]
		public bool ShowIntent { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("suggest_location_enrichment")]
		public bool SuggestLocationEnrichment { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("twitter_url")]
		public string TwitterUrl { get; set; }

		[JsonProperty("typed_custom_fields")]
		public TypedCustomFields TypedCustomFields { get; set; }

		[JsonProperty("website_url")]
		public string WebsiteUrl { get; set; }

		#endregion

	}
}