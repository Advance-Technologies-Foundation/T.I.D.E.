using System;

namespace MrktApolloApp.DomainModels
{
	/// <summary>
	/// Represents enriched person data
	/// </summary>
	/// <remarks>
	/// All members initialized to it default values.
	/// </remarks>
	public class EnrichedPerson
	{

		#region Properties: Public

		/// <value>
		/// Unique identifier of a person (PersonId)
		/// </value>
		public string ApolloId { get; set; } = string.Empty;

		/// <value>
		/// Id of a City found in <see cref="MrktApolloApp.Repository.Models.City"/>
		/// </value>
		public Guid City { get; set; } = Guid.Empty;

		/// <value>
		/// Id of a Country found in <see cref="MrktApolloApp.Repository.Models.Country"/>
		/// </value>
		public Guid Country { get; set; } = Guid.Empty;

		/// <value>
		/// Url of a person's Facebook profile
		/// </value>
		public string FacebookUrl { get; set; } = string.Empty;

		/// <value>
		/// First name of a person
		/// </value>
		public string FirstName { get; set; } = string.Empty;

		/// <value>
		/// Start date of a person's last employment with the organization
		/// </value>
		public DateTime LastEmploymentStartDate { get; set; } = DateTime.MinValue;

		/// <value>
		/// Job title of a person's last employment with the organization
		/// </value>
		public string LastEmploymentTitle { get; set; } = string.Empty;

		/// <value>
		/// Last name of a person
		/// </value>
		public string LastName { get; set; } = string.Empty;

		/// <value>
		/// Url of a person's Linkedin profile
		/// </value>
		public string LinkedinUrl { get; set; } = string.Empty;

		/// <value>
		/// Unique identifier of an organization a person relates to
		/// </value>
		public string OrganizationId { get; set; } = string.Empty;

		/// <value>
		/// Sanitized phone number of a person,
		/// </value>
		public string Phone { get; set; } = string.Empty;

		/// <value>
		/// Id of a Region found in <see cref="MrktApolloApp.Repository.Models.Region"/>
		/// </value>
		public Guid Region { get; set; } = Guid.Empty;

		/// <value>
		/// Current Job title
		/// </value>
		public string Title { get; set; } = string.Empty;

		/// <value>
		/// Url of a person's Twitter profile
		/// </value>
		public string TwitterUrl { get; set; } = string.Empty;

		/// <value>
		/// Corporate email when available
		/// </value>
		public string Email { get; set; } = string.Empty;

		/// <value>
		/// Personal email when available
		/// </value>
		public string PersonalEmail { get; set; } = string.Empty;
		
		
		public string ErrorMessage{get; set;}
		
		#endregion

	}
}