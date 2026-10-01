using System;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.ClientLink.Request
{
    public record ClientLinkRequest
    {
        /// <summary>
        /// Personal data of your customer which is required for this endpoint.
        /// </summary>
        public required ClientLinkOwner Owner { get; set; }

        /// <summary>
        /// Name of the organization.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Address of the organization. Note that the country parameter
        /// must always be provided.
        /// </summary>
        public required AddressObject Address { get; set; }

        /// <summary>
        /// The Chamber of Commerce (or local equivalent) registration number
        /// of the organization.
        /// </summary>
        public string? RegistrationNumber { get; set; }

        /// <summary>
        /// The VAT number of the organization, if based in the European Union
        /// or the United Kingdom.
        /// </summary>
        public string? VatNumber { get; set; }

        /// <summary>
        /// The legal entity type of the organization, based on its country of origin, for example nl-bv. See
        /// https://docs.mollie.com/reference/common-data-types#legal-entity for all possible values.
        /// </summary>
        public string? LegalEntity { get; set; }

        /// <summary>
        /// The registration office that the organization was registered at, for example aachen. See
        /// https://docs.mollie.com/reference/common-data-types#registration-office for all possible values.
        /// </summary>
        public string? RegistrationOffice { get; set; }

        /// <summary>
        /// The incorporation date of the organization.
        /// </summary>
#if NET8_0_OR_GREATER
        public DateOnly? IncorporationDate { get; set; }
#else
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? IncorporationDate { get; set; }
#endif
    }
}
