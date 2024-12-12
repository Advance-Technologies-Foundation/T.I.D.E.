using System;
using System.Globalization;
using Newtonsoft.Json;

namespace MrktApolloApp.Dto.Converters
{
	public class ShortDateConverter : JsonConverter
	{

		#region Constants: Private

		private const string DateFormat = "yyyy-MM-dd";

		#endregion

		#region Methods: Public

		public override bool CanConvert(Type objectType){
			return objectType == typeof(DateTime);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer){
			if (reader.Value == null) {
				return DateTime.MinValue.Date;
			}
			bool isParsed = DateTime.TryParseExact(
				reader.Value.ToString(),
				DateFormat,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out DateTime result);

			return isParsed ? result : DateTime.MinValue.Date;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer){
			if(value != null) {
				writer.WriteValue(((DateTime)value).ToString(DateFormat));
			}
		}

		#endregion

	}
}