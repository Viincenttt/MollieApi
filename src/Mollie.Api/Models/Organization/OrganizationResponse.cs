using Newtonsoft.Json;

namespace Mollie.Api.Models.Organization {
    public record OrganizationResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a organization object.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        ///     The organization's identifier, for example org_1234567.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The organization's official name.
        /// </summary>
        public required string Name { get; init; }
        
        /// <summary>
        /// The organization's email.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The preferred locale of the merchant which has been set in Mollie Dashboard.
        /// </summary>
        public required string Locale { get; init; }

        /// <summary>
        /// The address of the organization.
        /// </summary>
        public required AddressObject Address { get; init; }

        /// <summary>
        /// The registration number of the organization at the (local) chamber of commerce.
        /// </summary>
        public required string RegistrationNumber { get; init; }

        /// <summary>
        /// The VAT number of the organization, if based in the European Union. The VAT number has been checked with the VIES by Mollie.
        /// </summary>
        public required string VatNumber { get; init; }

        /// <summary>
        /// The organization’s VAT regulation, if based in the European Union. Either shifted (VAT is shifted) or dutch (Dutch VAT rate).
        /// </summary>
        public required string VatRegulation { get; init; }

        /// <summary>
        /// An object with several URL objects relevant to the organization. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required OrganizationResponseLinks Links { get; init; }
    }
}