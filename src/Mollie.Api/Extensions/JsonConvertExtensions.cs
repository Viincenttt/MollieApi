using Mollie.Api.ContractResolvers;
using Newtonsoft.Json;

namespace Mollie.Api.Extensions {
    internal static class JsonConvertExtensions {
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