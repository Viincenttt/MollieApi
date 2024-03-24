using Newtonsoft.Json;

namespace Mollie.Api.Models.Profile.Response {
    public class EnableGiftCardIssuerResponse {
        /// <summary>
        /// Indicates the response contains an issuer object. Will always contain issuer for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The unique identifier of the gift card issuer.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The full name of the gift card issuer.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// The status that the issuer is in. Possible values: pending-issuer or activated.
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required EnableGiftCardIssuerResponseLinks Links { get; init; }
    }
}
