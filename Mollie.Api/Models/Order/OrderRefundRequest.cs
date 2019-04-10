using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class OrderRefundRequest {
        /// <summary>
        /// An array of objects containing the order line details you want to create a refund for. If you send
        /// an empty array, the entire order will be refunded.
        /// </summary>
        public IEnumerable<OrderLineDetails> Lines { get; set; }

        /// <summary>
        /// The description of the refund you are creating. This will be shown to the consumer on their card or
        /// bank statement when possible. Max. 140 characters.
        /// </summary>
        public string Description { get; set; }
    }
}