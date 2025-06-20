using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class RawJsonConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        if (document.RootElement.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        if (document.RootElement.ValueKind == JsonValueKind.String)
        {
            return document.RootElement.GetString();
        }

        return document.RootElement.GetRawText();
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
            writer.WriteRawValue(value);
        }
        else
        {
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
