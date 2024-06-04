using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.PaymentLink.Response {
    public record PaymentLinkResponseLinks {
        /// <summary>
        /// The API resource URL of the payment link itself.
        /// </summary>
        public required UrlObjectLink<PaymentLinkResponse> Self { get; set; }

        /// <summary>
        /// Direct link to the payment link.
        /// </summary>
        public required UrlLink PaymentLink { get; set; }

        /// <summary>
        ///The URL to the payment link retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
