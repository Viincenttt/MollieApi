using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Organization {
    public record OrganizationResponseLinks {
        /// <summary>
        /// The API resource URL of the organization itself.
        /// </summary>
        public required UrlObjectLink<OrganizationResponse> Self { get; set; }

        /// <summary>
        /// The URL to the organization dashboard
        /// </summary>
        public required UrlLink Dashboard { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
