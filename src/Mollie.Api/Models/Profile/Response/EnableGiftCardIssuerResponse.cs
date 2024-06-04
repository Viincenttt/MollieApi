using Newtonsoft.Json;

namespace Mollie.Api.Models.Profile.Response {
    public record EnableGiftCardIssuerResponse {
        /// <summary>
        /// Indicates the response contains an issuer object. Will always contain issuer for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The unique identifier of the gift card issuer.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The full name of the gift card issuer.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The status that the issuer is in. Possible values: pending-issuer or activated.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required EnableGiftCardIssuerResponseLinks Links { get; set; }
    }
}
