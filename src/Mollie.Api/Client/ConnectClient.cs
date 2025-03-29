using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Connect.Request;
using Mollie.Api.Models.Connect.Response;

namespace Mollie.Api.Client {
    public class ConnectClient : BaseMollieClient, IConnectClient {
        private readonly string AuthorizeEndPoint;
        private readonly string TokenEndPoint;

        private readonly string _clientId;
        private readonly string _clientSecret;

        public ConnectClient(string clientId, string clientSecret, HttpClient? httpClient = null, string tokenEndPoint = "https://api.mollie.com/oauth2/", string authorizeEndPoint = "https://my.mollie.com/oauth2/authorize") : base(httpClient, tokenEndPoint) {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret)) {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            AuthorizeEndPoint = authorizeEndPoint;
            TokenEndPoint = tokenEndPoint;
            _clientSecret = clientSecret;
            _clientId = clientId;
        }

        public string GetAuthorizationUrl(
            string state,
            List<string> scopes,
            string? redirectUri = null,
            bool forceApprovalPrompt = false,
            string? locale = null,
            string? landingPage = null) {

            var parameters = new Dictionary<string, string> {
                {"client_id", _clientId},
                {"state", state},
                {"scope", string.Join(" ", scopes)},
                {"response_type", "code"},
                {"approval_prompt", forceApprovalPrompt ? "force" : "auto"}
            };
            parameters.AddValueIfNotNullOrEmpty("redirect_uri", redirectUri);
            parameters.AddValueIfNotNullOrEmpty("locale", locale);
            parameters.AddValueIfNotNullOrEmpty("landing_page", landingPage);

            return AuthorizeEndPoint + parameters.ToQueryString();
        }

        public async Task<TokenResponse> GetAccessTokenAsync(TokenRequest request) {
            return await PostAsync<TokenResponse>("tokens", request).ConfigureAwait(false);
        }

        public async Task RevokeTokenAsync(RevokeTokenRequest request) {
            await DeleteAsync("tokens", request).ConfigureAwait(false);
        }

        protected override HttpRequestMessage CreateHttpRequest(HttpMethod method, string relativeUri, HttpContent? content = null) {
            HttpRequestMessage httpRequest = new HttpRequestMessage(method, new Uri(new Uri(TokenEndPoint), relativeUri));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode($"{_clientId}:{_clientSecret}"));
            httpRequest.Content = content;

            return httpRequest;
        }

        private string Base64Encode(string value) {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}
