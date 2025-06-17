using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.JsonConverters {
    internal class MicrosecondEpochConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            var longValue = long.Parse(reader.Value!.ToString());
            return DateTimeOffset.FromUnixTimeMilliseconds(longValue).UtcDateTime;
        }
    }
}
