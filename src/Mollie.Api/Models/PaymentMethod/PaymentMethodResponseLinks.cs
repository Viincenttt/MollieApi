using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.PaymentMethod {
    public class PaymentMethodResponseLinks {
        /// <summary>
        /// The API resource URL of the payment method itself.
        /// </summary>
        public required UrlObjectLink<PaymentMethodResponse> Self { get; init; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}