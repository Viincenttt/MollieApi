using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Refund {
    public class RefundRequest {
        /// <summary>
        /// The amount to refund. For some payments, it can be up to €25.00 more than the original transaction amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank
        /// statement when possible. Max 140 characters.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the refund. Whenever 
        /// you fetch the refund with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// Set this to true to refund a test mode payment.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}