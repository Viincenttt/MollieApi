using System.Text.Json.Serialization;
using Mollie.Api.Models.Issuer.Response;

namespace Mollie.Api.Models.Profile.Response {
    public record EnableVoucherIssuerResponse {
        /// <summary>
        /// Indicates the response contains an issuer object. Will always contain issuer for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The unique identifier of the voucher issuer.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The full name of the voucher issuer.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// URLs of images representing the voucher issuer.
        /// </summary>
        public required IssuerResponseImage Image { get; set; }

        /// <summary>
        /// The status that the issuer is in. Possible values: pending-issuer or activated.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Information regarding the contractor.
        /// </summary>
        public EnableVoucherIssuerResponseContractor? Contractor { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the voucher issuer. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required EnableVoucherIssuerResponseLinks Links { get; set; }
    }
}
