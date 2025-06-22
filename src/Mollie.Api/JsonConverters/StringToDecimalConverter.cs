using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class StringToDecimalConverter : JsonConverter<decimal> {
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Null) {
            throw new JsonException("Cannot convert null to decimal.");
        }

        if (reader.TokenType == JsonTokenType.String) {
            string value = reader.GetString() ?? "0";
            if (decimal.TryParse(value, out decimal result)) {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number) {
            return reader.GetDecimal();
        }

        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeToConvert.Name}.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString("G", System.Globalization.CultureInfo.InvariantCulture));
    }
}
