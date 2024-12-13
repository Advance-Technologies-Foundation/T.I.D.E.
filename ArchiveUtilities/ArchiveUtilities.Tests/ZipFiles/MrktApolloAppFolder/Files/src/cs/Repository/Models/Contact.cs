#pragma warning disable CS8618, // Non-nullable field is uninitialized.

using System;
using ATF.Repository;
using ATF.Repository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MrktApolloApp.Repository.Models
{

	[ExcludeFromCodeCoverage]
	[Schema("Contact")]
	public class Contact: BaseModel
	{

		[SchemaProperty("Name")]
		public string Name { get; set; }

		
		
		[SchemaProperty("Account")]
		public Guid AccountId { get; set; }

		[LookupProperty("Account")]
		public virtual Account Account { get; set; }
		

		[SchemaProperty("JobTitle")]
		public string JobTitle { get; set; }

		[SchemaProperty("Department")]
		public Guid DepartmentId { get; set; }

		[LookupProperty("Department")]
		public virtual Department Department { get; set; }

		[SchemaProperty("Email")]
		public string Email { get; set; }

		[SchemaProperty("ContactPhoto")]
		public byte[] ContactPhoto { get; set; }

		[SchemaProperty("Photo")]
		public string PhotoId { get; set; }

		[LookupProperty("Photo")]
		public virtual SysImage Photo { get; set; }

		[SchemaProperty("Surname")]
		public string Surname { get; set; }

		[SchemaProperty("GivenName")]
		public string GivenName { get; set; }

		[SchemaProperty("Language")]
		public Guid LanguageId { get; set; }

		[LookupProperty("Language")]
		public virtual SysLanguage Language { get; set; }
		

		/// <summary>
		/// This field shows the date of the most recent enrichment from Apollo.io. If empty, data isn't synced with Apollo.io yet.
		/// </summary>
		[SchemaProperty("MrktApolloLastSyncDate")]
		public DateTime MrktApolloLastSyncDate { get; set; }

		/// <summary>
		/// Apollo unique identifier
		/// </summary>
		[SchemaProperty("MrktApolloId")]
		public string MrktApolloId { get; set; }
		

	}
}
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
