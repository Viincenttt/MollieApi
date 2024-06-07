using System.Collections.Generic;
using Mollie.Api.Models.Refund.Response;

namespace Mollie.Api.Models.Order.Response {
    public record OrderRefundResponse : RefundResponse {
        /// <summary>
        /// The unique identifier of the order this refund was created for. For example: ord_stTC2WHAuS.
        /// </summary>
        public required string OrderId { get; set; }

        /// <summary>
        /// An array of order line objects as described in Get order. Only available if the refund was created via the
        /// Create Order Refund API.
        /// </summary>
        public required IEnumerable<OrderLineResponse> Lines { get; set; }
    }
}
