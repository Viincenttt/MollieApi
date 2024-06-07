namespace Mollie.Api.Models.Onboarding.Request {
    public record SubmitOnboardingDataRequest {
        /// <summary>
        /// Data of the organization you want to provide.
        /// </summary>
        public OnboardingOrganizationRequest? Organization { get; set; }

        /// <summary>
        /// Data of the payment profile you want to provide.
        /// </summary>
        public OnboardingProfileRequest? Profile { get; set; }
    }
}
