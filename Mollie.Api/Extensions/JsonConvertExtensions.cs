using Mollie.Api.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mollie.Api.Extensions {
    public static class JsonConvertExtensions {
        public static string SerializeObjectCamelCase(object value) {
            return JsonConvert.SerializeObject(value,
                new JsonSerializerSettings {
                    DateFormatString = "yyyy-MM-dd",
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public static string SerializeObjectSnakeCase(object value) {
            return JsonConvert.SerializeObject(value,
                new JsonSerializerSettings {
                    DateFormatString = "yyyy-MM-dd",
                    ContractResolver = new SnakeCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}