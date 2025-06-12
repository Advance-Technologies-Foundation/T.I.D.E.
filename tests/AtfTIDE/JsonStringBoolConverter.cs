using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtfTIDE
{
    /// <summary>
    /// Custom JSON converter that converts string values like "true" and "false" to boolean values.
    /// </summary>
    public class JsonStringBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                // Get the string value
                string stringValue = reader.GetString();
                
                // Try to convert the string to a boolean
                if (bool.TryParse(stringValue, out bool result))
                {
                    return result;
                }
                
                // If the string can't be directly parsed, handle specific string values
                if (string.Equals("true", stringValue, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                
                if (string.Equals("false", stringValue, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                
                // For any other string values, return false
                return false;
            }
            
            // For boolean tokens, read directly
            if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            
            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            
            // For unexpected token types, throw an exception
            throw new JsonException($"Unable to convert {reader.TokenType} to Boolean");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            // Write the boolean value as a string
            writer.WriteStringValue(value.ToString().ToLowerInvariant());
        }
    }
}
