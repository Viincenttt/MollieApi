using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Order.Request {
    public record OrderRefundRequest {
        /// <summary>
        /// An array of objects containing the order line details you want to create a refund for. If you send
        /// an empty array, the entire order will be refunded.
        /// </summary>
        public required IEnumerable<OrderLineDetails> Lines { get; init; }

        /// <summary>
        /// The description of the refund you are creating. This will be shown to the consumer on their card or
        /// bank statement when possible. Max. 140 characters.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The optional metadata you provided upon line creation.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
