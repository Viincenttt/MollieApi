﻿using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Connect.Request {
    public record RevokeTokenRequest {
        /// <summary>
        /// Type of the token you want to revoke.
        /// </summary>
        [JsonPropertyName("token_type_hint")]
        public required string TokenTypeHint { get; set; }

        /// <summary>
        /// The token you want to revoke
        /// </summary>
        public required string Token { get; set; }
    }
}
