using System.Collections.Generic;
using Mollie.Api.Models.Onboarding.Response;
using Mollie.Api.Models.Organization;

namespace Mollie.Api.Models.Client.Response {
    public record ClientEmbeddedResponse {

        public OrganizationResponse? Organization { get; set; }

        public IEnumerable<OnboardingStatusResponse>? Onboarding { get; set; }

    }
}
