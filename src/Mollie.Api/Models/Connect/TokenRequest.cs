using Newtonsoft.Json;

namespace Mollie.Api.Models.Connect {
    public record TokenRequest {
        /// <param name="code">This can be an authorization code or a refresh token. The correct grant type will be automatically selected</param>
        /// <param name="redirectUri">The URL the merchant is sent back to once the request has been authorized. It must match the URL you set when registering your app. </param>
        public TokenRequest(string code, string redirectUri) {
            if (code.StartsWith("refresh_")) {
                GrantType = "refresh_token";
                RefreshToken = code;
            }
            else {
                GrantType = "authorization_code";
                Code = code;
            }
            RedirectUri = redirectUri;
        }

        /// <summary>
        ///     If you wish to exchange your auth code for an access token, use grant type authorization_code. If you wish to renew
        ///     your access token with your refresh token, use grant type refresh_token.
        ///     Possible values: authorization_code refresh_token
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; }

        /// <summary>
        ///     Optional – The auth code you've received when creating the authorization. Only use this field when using grant type
        ///     authorization_code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        ///     Optional – The refresh token you've received when creating the authorization. Only use this field when using grant
        ///     type refresh_token.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        /// <summary>
        ///     The URL the merchant is sent back to once the request has been authorized. It must match the URL you set when
        ///     registering your app.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string? RedirectUri { get; set; }
    }
}