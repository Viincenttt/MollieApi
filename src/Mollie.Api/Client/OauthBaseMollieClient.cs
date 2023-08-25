using System;
using System.Net.Http;

namespace Mollie.Api.Client {
    public class OauthBaseMollieClient : BaseMollieClient {
        public OauthBaseMollieClient(string oauthAccessToken, HttpClient httpClient = null) : base(oauthAccessToken, httpClient) {
            if (string.IsNullOrWhiteSpace(oauthAccessToken)) {
                throw new ArgumentNullException(nameof(oauthAccessToken), "Mollie API key cannot be empty");
            }

            this.ValidateApiKeyIsOauthAccesstoken(true);
        }
    }
}