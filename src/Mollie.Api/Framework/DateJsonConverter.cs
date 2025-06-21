using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.Framework;

/// <summary>
/// Custom converter to handle date format yyyy-MM-dd in System.Text.Json.
/// </summary>
public class DateJsonConverter : JsonConverter<DateTime?>
{
    private const string Format = "yyyy-MM-dd";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Null) {
            return null;
        }

        var value = reader.GetString();
        if (value == null) {
            throw new JsonException("Expected date string value.");
        }

        return DateTime.ParseExact(value, Format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null) {
            writer.WriteNullValue();
        } else {
            writer.WriteStringValue(value.Value.ToString(Format));
        }
    }
}
