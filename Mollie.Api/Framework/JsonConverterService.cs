using System.Collections.Generic;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mollie.Api.Framework {
    public class JsonConverterService {
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;

        public JsonConverterService() {
            this._defaultJsonDeserializerSettings = this.CreateDefaultJsonDeserializerSettings();
        }

        public string Serialize(object objectToSerialize) {
            return JsonConvert.SerializeObject(objectToSerialize,
                new JsonSerializerSettings {
                    DateFormatString = "yyyy-MM-dd",
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                });
        }

        public T Deserialize<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, this._defaultJsonDeserializerSettings);
        }

        /// <summary>
        ///     Creates the default Json serial settings for the JSON.NET parsing.
        /// </summary>
        private JsonSerializerSettings CreateDefaultJsonDeserializerSettings() {
            return new JsonSerializerSettings {
                DateFormatString = "yyyy-MM-dd",
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> {
                    // Add a special converter for payment responses, because we need to create specific classes based on the payment method
                    new PaymentResponseConverter(new PaymentResponseFactory()),
                    // Add a special converter for the balance report responses, because we need to create specific classes based on the report grouping
                    new BalanceReportResponseJsonConverter(new BalanceReportResponseFactory()),
                    // Add a special converter for the balance transaction responses, because we need to create specific classes based on the transaction type
                    new BalanceTransactionJsonConverter(new BalanceTransactionFactory()),
                }
            };
        }
    }
}