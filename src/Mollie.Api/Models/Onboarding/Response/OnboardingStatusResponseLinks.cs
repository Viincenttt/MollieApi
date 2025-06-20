using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Onboarding.Response {
    /// <summary>
    /// An object with several URL objects relevant to the onboarding status. Every URL object will contain an href and a type field.
    /// </summary>
    public record OnboardingStatusResponseLinks {
        /// <summary>
        /// The API resource URL of this endpoint itself.
        /// </summary>
        public required UrlLink Self { get; set; }

        /// <summary>
        /// The URL of the onboarding process in Mollie Dashboard. You can redirect your customer to here for e.g. completing the onboarding process.
        /// </summary>
        public required UrlLink Dashboard { get; set; }

        /// <summary>
        /// The API resource URL of the organization.
        /// </summary>
        public UrlObjectLink<OrganizationResponse>? Organization { get; set; }

        /// <summary>
        /// The URL to the onboarding status retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
