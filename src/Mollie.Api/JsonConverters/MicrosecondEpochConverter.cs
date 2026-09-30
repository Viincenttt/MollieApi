using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters {
    internal class MicrosecondEpochConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var longValue = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeMilliseconds(longValue);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
