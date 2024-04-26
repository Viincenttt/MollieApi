namespace Mollie.Api.Models.Onboarding.Request {
    /// <summary>
    /// Data of the organization you want to provide.
    /// </summary>
    public class OnboardingOrganizationRequest {
        /// <summary>
        /// Name of the organization.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Address of the organization.
        /// </summary>
        public AddressObject? Address { get; set; }
        
        /// <summary>
        /// The Chamber of Commerce (or local equivalent) registration number of the organization.
        /// </summary>
        public string? RegistrationNumber { get; set; }
        
        /// <summary>
        /// The VAT number of the organization, if based in the European Union or the United Kingdom.
        /// </summary>
        public string? VatNumber { get; set; }
        
        /// <summary>
        /// The organization’s VAT regulation, if based in the European Union. Either shifted (VAT is shifted)
        /// or dutch (Dutch VAT rate) is accepted.
        /// </summary>
        public string? VatRegulation { get; set; }
    }
}
