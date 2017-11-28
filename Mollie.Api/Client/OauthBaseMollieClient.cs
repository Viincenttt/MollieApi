using System;

namespace Mollie.Api.Client {
    public class OauthBaseMollieClient : BaseMollieClient {
        public OauthBaseMollieClient(string oauthAccessToken) : base(oauthAccessToken) {
            if (string.IsNullOrWhiteSpace(oauthAccessToken)) {
                throw new ArgumentNullException(nameof(oauthAccessToken), "Mollie API key cannot be empty");
            }

            this.ValidateApiKeyIsOauthAccesstoken(true);
        }
    }
}