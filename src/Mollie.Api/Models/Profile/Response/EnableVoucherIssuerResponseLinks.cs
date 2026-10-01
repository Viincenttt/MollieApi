using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public record EnableVoucherIssuerResponseLinks {
        /// <summary>
        /// The API resource URL of the voucher issuer itself.
        /// </summary>
        public required UrlLink Self { get; set; }

        /// <summary>
        /// The URL to the voucher issuer endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
