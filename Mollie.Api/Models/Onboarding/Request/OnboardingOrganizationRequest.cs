namespace Mollie.Api.Models.Onboarding.Request {
    /// <summary>
    /// Data of the organization you want to provide.
    /// </summary>
    public class OnboardingOrganizationRequest {
        /// <summary>
        /// Name of the organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address of the organization.
        /// </summary>
        public AddressObject Address { get; set; }
    }
}
