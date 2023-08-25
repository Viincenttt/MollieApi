using Newtonsoft.Json;

namespace Mollie.Api.Models.Connect {
    public class RevokeTokenRequest {
        /// <summary>
        /// Type of the token you want to revoke.
        /// </summary>
        [JsonProperty("token_type_hint")]
        public string TokenTypeHint { get; set; }

        /// <summary>
        /// The token you want to revoke
        /// </summary>
        public string Token { get; set; }
    }
}
