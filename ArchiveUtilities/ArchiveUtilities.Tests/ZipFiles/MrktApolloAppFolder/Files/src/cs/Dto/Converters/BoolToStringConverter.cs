using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto.Converters
{
	public class BoolToStringConverter : JsonConverter<bool>
	{

		private readonly IEnumerable<string> _applyToProp;

		public BoolToStringConverter(IEnumerable<string> applyToProp){
			_applyToProp = applyToProp;
		}
		public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer){
		
			if (_applyToProp.Contains(writer.Path)) {
				writer.WriteValue(value ? "yes" : "no");
			}
		}

		public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
			JsonSerializer serializer) {
			
			return reader.Value?.ToString() == "yes";
		}

	}
}