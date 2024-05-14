using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Mollie.Api.Models.Settlement.Response;

namespace Mollie.Api.JsonConverters {
    internal class SettlementPeriodConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            // If we have no periods, Mollie returns a empty array instead of an object
            JToken token = JToken.Load(reader);
            if (token is JArray) {
                return new Dictionary<int, Dictionary<int, SettlementPeriod>>();
            }

            existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator!();
            serializer.Populate(token.CreateReader(), existingValue);
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
