using System;
using Newtonsoft.Json;
using Terrasoft.Common.Json;

namespace MrktApolloApp.Dto
{
	internal class WebSocket
	{

		#region Constructors: Public
		public WebSocket(string commandName, string schemaName, Guid recordId, string message) {
			CommandName = commandName;
			SchemaName = schemaName;
			RecordId = recordId;
			Message = message;
		}

		#endregion

		#region Properties: Public

		[JsonProperty("commandName")]
		public string CommandName { get; private set; }

		[JsonProperty("message")]
		public string Message { get; private set; }

		[JsonProperty("recordId")]
		public Guid RecordId { get; private set; }

		[JsonProperty("schemaName")]
		public string SchemaName { get; private set; }

		#endregion

		#region Methods: Public

		public override string ToString(){
			return Json.FormatJsonString(Json.Serialize(this), Formatting.Indented);
		} 
		

		#endregion
	}
}