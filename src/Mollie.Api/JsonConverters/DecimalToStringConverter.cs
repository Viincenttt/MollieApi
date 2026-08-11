using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class DecimalToStringConverter : JsonConverter<decimal> {
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Null) {
            throw new JsonException($"Cannot convert null to {typeToConvert.Name}.");
        }

        if (reader.TokenType == JsonTokenType.String) {
            string value = reader.GetString()!;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result)) {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number) {
            return reader.GetDecimal();
        }

        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeToConvert.Name}.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
