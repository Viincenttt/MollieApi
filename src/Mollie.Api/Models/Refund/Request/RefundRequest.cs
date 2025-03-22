using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Refund.Request {
    public record RefundRequest {
        /// <summary>
        /// The amount to refund. For some payments, it can be up to €25.00 more than the original transaction amount.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank
        /// statement when possible. Max 140 characters.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the refund. Whenever
        /// you fetch the refund with our API, we’ll also include the metadata. You can use up to approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// With Mollie Connect you can charge fees on payments that your app is processing on behalf of other Mollie merchants,
        /// by providing the routing object during payment creation. When creating refunds for these routed payments, by default
        /// the full amount is deducted from your balance.If you want to pull back the funds that were routed to the connected
        /// merchant(s), you can set this parameter to true when issuing a full refund. For more fine-grained control and for
        /// partial refunds, use the routingReversals parameter instead.
        /// </summary>
        public bool? ReverseRouting { get; set; }

        /// <summary>
        /// When creating refunds for routed payments, by default the full amount is deducted from your balance. If you want to
        /// pull back funds from the connected merchant(s), you can use this parameter to specify what amount needs to be
        /// reversed from which merchant(s). If you simply want to fully reverse the routed funds, you can also use the
        /// reverseRouting parameter instead.
        /// </summary>
        public IList<RoutingReversal>? RoutingReversals { get; set; }

        /// <summary>
        /// Set this to true to refund a test mode payment.
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerSettings? jsonSerializerSettings = null) {
            Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
    }
}
