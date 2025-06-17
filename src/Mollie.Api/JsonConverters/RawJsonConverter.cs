using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class RawJsonConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the JSON token as a string (raw JSON or simple string)
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            // Return the raw JSON string of the token (including objects, arrays)
            return document.RootElement.GetRawText();
        }
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (string.IsNullOrEmpty(value))
        {
            writer.WriteNullValue();
            return;
        }

        if (IsValidJson(value))
        {
            // Write raw JSON by parsing and writing the element (since no WriteRawValue)
            using JsonDocument doc = JsonDocument.Parse(value);
            doc.RootElement.WriteTo(writer);
        }
        else
        {
            // Write as a normal string value
            writer.WriteStringValue(value);
        }
    }

    private bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
            (strInput.StartsWith("[") && strInput.EndsWith("]")))
        {
            try
            {
                JsonDocument.Parse(strInput);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
