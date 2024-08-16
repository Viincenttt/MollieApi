using System;
using System.Net.Http;
using Mollie.Api.Framework.Authentication.Abstract;

namespace Mollie.Api.Client {
    public class OauthBaseMollieClient : BaseMollieClient {
        protected OauthBaseMollieClient(string oauthAccessToken, HttpClient? httpClient = null)
            : base(oauthAccessToken, httpClient) {
            ValidateApiKeyIsOauthAccesstoken(oauthAccessToken);
        }

        protected OauthBaseMollieClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(mollieSecretManager, httpClient) {
            ValidateApiKeyIsOauthAccesstoken(mollieSecretManager.GetBearerToken());
        }

        private void ValidateApiKeyIsOauthAccesstoken(string oauthAccessToken) {
            if (string.IsNullOrWhiteSpace(oauthAccessToken)) {
                throw new ArgumentNullException(nameof(oauthAccessToken), "Mollie API key cannot be empty");
            }

            ValidateApiKeyIsOauthAccesstoken(true);
        }
    }
}
