using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class NullableDecimalToStringConverter : JsonConverter<decimal?> {
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Null) {
            return null;
        }

        return new DecimalToStringConverter().Read(ref reader, typeof(decimal), options);
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options) {
        if (value is null) {
            writer.WriteNullValue();
        }
        else {
            new DecimalToStringConverter().Write(writer, value.Value, options);
        }
    }
}

