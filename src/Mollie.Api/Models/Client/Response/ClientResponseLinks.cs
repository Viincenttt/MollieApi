using Mollie.Api.Models.Onboarding.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Client.Response {
    public record ClientResponseLinks {
        /// <summary>
        /// The API resource URL of the client itself.
        /// </summary>
        public required UrlObjectLink<ClientResponse> Self { get; set; }

        /// <summary>
        /// A link pointing to the product page in your web shop of the product sold.
        /// </summary>
        public required UrlObjectLink<OrganizationResponse> Organization { get; set; }

        /// <summary>
        /// The API resource URL of the client's onboarding status.
        /// </summary>
        public required UrlObjectLink<OnboardingStatusResponse> Onboarding { get; set; }

        /// <summary>
        /// The URL to the client retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
