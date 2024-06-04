using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.PaymentMethod.Response {
    public record PaymentMethodResponseLinks {
        /// <summary>
        /// The API resource URL of the payment method itself.
        /// </summary>
        public required UrlObjectLink<PaymentMethodResponse> Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
