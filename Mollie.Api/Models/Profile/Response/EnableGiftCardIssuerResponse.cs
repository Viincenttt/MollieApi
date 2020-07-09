using Newtonsoft.Json;

namespace Mollie.Api.Models.Profile.Response {
    public class EnableGiftCardIssuerResponse {
        /// <summary>
        /// Indicates the response contains an issuer object. Will always contain issuer for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The unique identifier of the gift card issuer.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The full name of the gift card issuer.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The status that the issuer is in. Possible values: pending-issuer or activated.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public EnableGiftCardIssuerResponseLinks Links { get; set; }
    }
}
