using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.DelayedRouting.Response {
    public record DelayedRoutingResponseLinks {
        /// <summary>
        /// The API resource URL of the route itself.
        /// </summary>
        public required UrlObjectLink<DelayedRoutingResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the payment this route belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; set; }

        /// <summary>
        /// The URL to the route retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}

