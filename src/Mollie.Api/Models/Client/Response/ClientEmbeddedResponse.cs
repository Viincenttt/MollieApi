using Mollie.Api.Models.Capability.Response;
using Mollie.Api.Models.Onboarding.Response;
using Mollie.Api.Models.Organization;

namespace Mollie.Api.Models.Client.Response {
    public record ClientEmbeddedResponse {

        public OrganizationResponse? Organization { get; set; }

        public OnboardingStatusResponse? Onboarding { get; set; }

        public CapabilityResponse? Capabilities { get; set; }
    }
}
