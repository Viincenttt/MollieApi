using Newtonsoft.Json;

namespace Mollie.Api.Models.Organization {
    public class OrganizationResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a organization object.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        ///     The organization's identifier, for example org_1234567.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The organization's official name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The organization's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The preferred locale of the merchant which has been set in Mollie Dashboard.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The address of the organization.
        /// </summary>
        public AddressObject Address { get; set; }

        /// <summary>
        /// The registration number of the organization at the (local) chamber of commerce.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// The VAT number of the organization, if based in the European Union. The VAT number has been checked with the VIES by Mollie.
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// The organization’s VAT regulation, if based in the European Union. Either shifted (VAT is shifted) or dutch (Dutch VAT rate).
        /// </summary>
        public string VatRegulation { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the organization. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public OrganizationResponseLinks Links { get; set; }
    }
}