using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Payment.Response {
    public record PaymentRoutingResponseLinks {
        /// <summary>
        /// The API resource URL of the route itself.
        /// </summary>
        public required UrlLink Self { get; set; }

        /// <summary>
        /// The API resource URL of the payment this route belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; set; }
    }
}
