using System;
using Newtonsoft.Json;

namespace Mollie.Api.JsonConverters {
    using Newtonsoft.Json.Linq;
    internal class ListResponseConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            throw new NotImplementedException("Not implemented");
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }
            else {
                // Find the first array object and deserialize it to the list we want
                JObject obj = JObject.Load(reader);
                if (obj.First?.First is JArray objectArray) {
                    return objectArray.ToObject(objectType, serializer);
                }
            }

            return null;
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) {
            return false;
        }
    }
}
