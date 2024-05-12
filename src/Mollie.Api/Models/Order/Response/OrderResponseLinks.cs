using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Order.Response {
    public record OrderResponseLinks {
        /// <summary>
        /// The API resource URL of the order itself.
        /// </summary>
        public required UrlObjectLink<OrderResponse> Self { get; init; }

        /// <summary>
        /// The URL your customer should visit to make the payment for the order.
        /// This is where you should redirect the customer to after creating the order.
        /// </summary>
        public UrlLink? Checkout { get; set; }

        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}
