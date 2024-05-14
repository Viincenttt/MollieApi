using Newtonsoft.Json;

namespace Mollie.Api.Models.Connect.Request {
    public record RevokeTokenRequest {
        /// <summary>
        /// Type of the token you want to revoke.
        /// </summary>
        [JsonProperty("token_type_hint")]
        public required string TokenTypeHint { get; init; }

        /// <summary>
        /// The token you want to revoke
        /// </summary>
        public required string Token { get; init; }
    }
}
