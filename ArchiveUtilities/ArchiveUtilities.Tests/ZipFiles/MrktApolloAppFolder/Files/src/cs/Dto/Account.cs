using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto
{
	[ExcludeFromCodeCoverage]
	[JsonObject]
	internal class Account
	{

		#region Properties: Public

		[JsonProperty("account_playbook_statuses")]
		public object[] AccountPlaybookStatuses { get; set; }

		[JsonProperty("account_rule_config_statuses")]
		public object[] AccountRuleConfigStatuses { get; set; }

		[JsonProperty("account_stage_id")]
		public string AccountStageId { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("creator_id")]
		public string CreatorId { get; set; }

		[JsonProperty("crm_owner_id")]
		public object CrmOwnerId { get; set; }

		[JsonProperty("domain")]
		public string Domain { get; set; }

		[JsonProperty("existence_level")]
		public string ExistenceLevel { get; set; }

		[JsonProperty("hubspot_id")]
		public object HubspotId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("intent_strength")]
		public object IntentStrength { get; set; }

		[JsonProperty("label_ids")]
		public object[] LabelIds { get; set; }

		[JsonProperty("modality")]
		public string Modality { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("organization_id")]
		public string OrganizationId { get; set; }

		[JsonProperty("original_source")]
		public string OriginalSource { get; set; }

		[JsonProperty("owner_id")]
		public string OwnerId { get; set; }

		[JsonProperty("parent_account_id")]
		public object ParentAccountId { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("phone_status")]
		public string PhoneStatus { get; set; }

		[JsonProperty("salesforce_id")]
		public object SalesforceId { get; set; }

		[JsonProperty("sanitized_phone")]
		public string SanitizedPhone { get; set; }

		[JsonProperty("show_intent")]
		public bool ShowIntent { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("typed_custom_fields")]
		public TypedCustomFields TypedCustomFields { get; set; }

		#endregion

	}
}