using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.Models.Settlement.Response;

namespace Mollie.Api.JsonConverters;

internal class SettlementPeriodConverter : JsonConverter<Dictionary<int, Dictionary<int, SettlementPeriod>>> {
    public override Dictionary<int, Dictionary<int, SettlementPeriod>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // If we have no periods, Mollie returns a empty array instead of an object
        if (reader.TokenType == JsonTokenType.StartArray) {
            reader.Skip();
            return new Dictionary<int, Dictionary<int, SettlementPeriod>>();
        }

        var result = new Dictionary<int, Dictionary<int, SettlementPeriod>>();
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        foreach (var property in doc.RootElement.EnumerateObject()) {
            int month = int.Parse(property.Name);
            var monthPeriods = new Dictionary<int, SettlementPeriod>();

            foreach (var period in property.Value.EnumerateObject()) {
                int day = int.Parse(period.Name);
                var settlementPeriod = JsonSerializer.Deserialize<SettlementPeriod>(period.Value.GetRawText(), options);
                monthPeriods[day] = settlementPeriod;
            }

            result[month] = monthPeriods;
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<int, Dictionary<int, SettlementPeriod>> value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}

