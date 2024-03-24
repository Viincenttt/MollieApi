using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.PaymentLink.Response {
    public class PaymentLinkResponseLinks {
        /// <summary>
        /// The API resource URL of the payment link itself.
        /// </summary>
        public required UrlObjectLink<PaymentLinkResponse> Self { get; init; }

        /// <summary>
        /// Direct link to the payment link.
        /// </summary>
        public required UrlLink PaymentLink { get; init; } 

        /// <summary>
        ///The URL to the payment link retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}