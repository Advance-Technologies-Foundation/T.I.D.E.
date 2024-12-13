using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Contact : IMappableDto
	{

		#region Properties: Public

		[JsonProperty("account_id")]
		public string AccountId { get; set; }

		[JsonProperty("account_phone_note")]
		public object AccountPhoneNote { get; set; }

		[JsonProperty("contact_emails")]
		public IEnumerable<ContactEmail> ContactEmails { get; set; }

		[JsonProperty("contact_roles")]
		public object[] ContactRoles { get; set; }

		[JsonProperty("contact_rule_config_statuses")]
		public object[] ContactRuleConfigStatuses { get; set; }

		[JsonProperty("contact_stage_id")]
		public object ContactStageId { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("creator_id")]
		public string CreatorId { get; set; }

		[JsonProperty("crm_id")]
		public object CrmId { get; set; }

		[JsonProperty("crm_owner_id")]
		public object CrmOwnerId { get; set; }

		[JsonProperty("direct_dial_enrichment_failed_at")]
		public object DirectDialEnrichmentFailedAt { get; set; }

		[JsonProperty("direct_dial_status")]
		public object DirectDialStatus { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("emailer_campaign_ids")]
		public object[] EmailerCampaignIds { get; set; }

		[JsonProperty("email_from_customer")]
		public object EmailFromCustomer { get; set; }

		[JsonProperty("email_needs_tickling")]
		public bool EmailNeedsTickling { get; set; }

		[JsonProperty("email_source")]
		public string EmailSource { get; set; }

		[JsonProperty("email_status")]
		public string EmailStatus { get; set; }

		[JsonProperty("email_status_unavailable_reason")]
		public object EmailStatusUnavailableReason { get; set; }

		[JsonProperty("email_true_status")]
		public string EmailTrueStatus { get; set; }

		[JsonProperty("email_unsubscribed")]
		public object EmailUnsubscribed { get; set; }

		[JsonProperty("existence_level")]
		public string ExistenceLevel { get; set; }

		[JsonProperty("extrapolated_email_confidence")]
		public object ExtrapolatedEmailConfidence { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("free_domain")]
		public bool FreeDomain { get; set; }

		[JsonProperty("has_email_arcgate_request")]
		public bool HasEmailArcgateRequest { get; set; }

		[JsonProperty("has_pending_email_arcgate_request")]
		public bool HasPendingEmailArcgateRequest { get; set; }

		[JsonProperty("headline")]
		public string Headline { get; set; }

		[JsonProperty("hubspot_company_id")]
		public object HubspotCompanyId { get; set; }

		[JsonProperty("hubspot_vid")]
		public object HubspotVid { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("label_ids")]
		public string[] LabelIds { get; set; }

		[JsonProperty("last_activity_date")]
		public string LastActivityDate { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("linkedin_uid")]
		public object LinkedinUid { get; set; }

		[JsonProperty("linkedin_url")]
		public string LinkedinUrl { get; set; }

		[JsonProperty("merged_crm_ids")]
		public object MergedCrmIds { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("organization_id")]
		public string OrganizationId { get; set; }

		[JsonProperty("organization_name")]
		public string OrganizationName { get; set; }

		[JsonProperty("original_source")]
		public string OriginalSource { get; set; }

		[JsonProperty("owner_id")]
		public string OwnerId { get; set; }

		[JsonProperty("person_id")]
		public string PersonId { get; set; }

		[JsonProperty("phone_numbers")]
		public IEnumerable<PhoneNumbers> PhoneNumbers { get; set; }

		[JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }

		[JsonProperty("present_raw_address")]
		public string PresentRawAddress { get; set; }

		[JsonProperty("queued_for_crm_push")]
		public bool QueuedForCrmPush { get; set; }

		[JsonProperty("salesforce_account_id")]
		public object SalesforceAccountId { get; set; }

		[JsonProperty("salesforce_contact_id")]
		public object SalesforceContactId { get; set; }

		[JsonProperty("salesforce_id")]
		public object SalesforceId { get; set; }

		[JsonProperty("salesforce_lead_id")]
		public object SalesforceLeadId { get; set; }

		[JsonProperty("sanitized_phone")]
		public string SanitizedPhone { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("suggested_from_rule_engine_config_id")]
		public object SuggestedFromRuleEngineConfigId { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("typed_custom_fields")]
		public TypedCustomFields TypedCustomFields { get; set; }

		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }

		[JsonProperty("updated_email_true_status")]
		public bool UpdatedEmailTrueStatus { get; set; }
		
		[JsonProperty("facebook_url")]
		public string FacebookUrl { get; set; }

		[JsonProperty("twitter_url")]
		public string TwitterUrl { get; set; }
		
		[JsonProperty("employment_history")]
		public IEnumerable<EmploymentHistory> EmploymentHistory { get; set; }

		
		#endregion

	}
}