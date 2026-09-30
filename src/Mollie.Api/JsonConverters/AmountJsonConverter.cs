using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Models;

namespace Mollie.Api.JsonConverters;

/// <summary>
/// Serializes the value of an <see cref="Amount"/> as a string with the number of decimals Mollie expects for the
/// currency, for example "20.00" for EUR and "20" for JPY. Only zeros are added or removed, the value is never rounded.
/// </summary>
internal class AmountJsonConverter : JsonConverter<Amount> {
    public override Amount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartObject) {
            throw new JsonException($"Unable to convert a token of type {reader.TokenType} to {nameof(Amount)}.");
        }

        string? currency = null;
        decimal? value = null;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject) {
            string? propertyName = reader.GetString();
            reader.Read();

            if (string.Equals(propertyName, "currency", StringComparison.OrdinalIgnoreCase)) {
                currency = reader.GetString();
            }
            else if (string.Equals(propertyName, "value", StringComparison.OrdinalIgnoreCase)) {
                value = ReadValue(ref reader);
            }
            else {
                reader.Skip();
            }
        }

        if (currency == null || value == null) {
            throw new JsonException($"An {nameof(Amount)} requires both a currency and a value.");
        }

        return new Amount(currency, value.Value);
    }

    public override void Write(Utf8JsonWriter writer, Amount amount, JsonSerializerOptions options) {
        writer.WriteStartObject();
        writer.WriteString("currency", amount.Currency);
        writer.WriteString("value", amount.ToFormattedValue());
        writer.WriteEndObject();
    }

    private static decimal ReadValue(ref Utf8JsonReader reader) {
        if (reader.TokenType == JsonTokenType.Number) {
            return reader.GetDecimal();
        }

        if (reader.TokenType == JsonTokenType.String &&
            decimal.TryParse(reader.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result)) {
            return result;
        }

        throw new JsonException($"Unable to convert the value of an {nameof(Amount)} to a decimal.");
    }
}
