namespace Mollie.Api.Models.Connect {
    public class TokenRequest {
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
        public string GrantType { get; }

        /// <summary>
        ///     Optional – The auth code you've received when creating the authorization. Only use this field when using grant type
        ///     authorization_code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     Optional – The refresh token you've received when creating the authorization. Only use this field when using grant
        ///     type refresh_token.
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        ///     The URL the merchant is sent back to once the request has been authorized. It must match the URL you set when
        ///     registering your app.
        /// </summary>
        public string RedirectUri { get; }
    }
}