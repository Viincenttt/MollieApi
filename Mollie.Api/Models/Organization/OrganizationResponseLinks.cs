using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Organization {
    public class OrganizationResponseLinks {
        /// <summary>
        /// The API resource URL of the organization itself.
        /// </summary>
        public UrlObjectLink<OrganizationResponse> Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
    }
}