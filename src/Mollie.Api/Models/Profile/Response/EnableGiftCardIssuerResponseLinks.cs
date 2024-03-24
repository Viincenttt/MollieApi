using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public class EnableGiftCardIssuerResponseLinks {
        /// <summary>
        /// The API resource URL of the gift card issuer itself.
        /// </summary>
        public required UrlLink Self { get; init; }

        /// <summary>
        /// The URL to the gift card issuer retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}
