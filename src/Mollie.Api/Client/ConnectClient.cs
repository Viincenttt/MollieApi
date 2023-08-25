using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Connect;

namespace Mollie.Api.Client {
    public class ConnectClient : BaseMollieClient, IConnectClient {
        public const string AuthorizeEndPoint = "https://www.mollie.com/oauth2/authorize";
        public const string TokenEndPoint = "https://api.mollie.nl/oauth2/";
        private readonly string _clientId;
        private readonly string _clientSecret;

        public ConnectClient(string clientId, string clientSecret, HttpClient httpClient = null): base(httpClient, ConnectClient.TokenEndPoint) {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret)) {
                throw new ArgumentNullException(nameof(clientSecret));
            }
            
            this._clientSecret = clientSecret;
            this._clientId = clientId;
        }

        public string GetAuthorizationUrl(string state, List<string> scopes, string redirectUri = null, bool forceApprovalPrompt = false, string locale = null) {
            var parameters = new Dictionary<string, string> {
                {"client_id", this._clientId},
                {"state", state},
                {"scope", string.Join(" ", scopes)},
                {"response_type", "code"},
                {"approval_prompt", forceApprovalPrompt ? "force" : "auto"}
            };
            parameters.AddValueIfNotNullOrEmpty("redirect_uri", redirectUri);
            parameters.AddValueIfNotNullOrEmpty("locale", locale);

            return AuthorizeEndPoint + parameters.ToQueryString();
        }

        public async Task<TokenResponse> GetAccessTokenAsync(TokenRequest request) {
            return await this.PostAsync<TokenResponse>("tokens", request).ConfigureAwait(false);
        }

        public async Task RevokeTokenAsync(RevokeTokenRequest request) {
            await this.DeleteAsync("tokens", request).ConfigureAwait(false);
        }

        protected override HttpRequestMessage CreateHttpRequest(HttpMethod method, string relativeUri, HttpContent content = null) {
            HttpRequestMessage httpRequest = new HttpRequestMessage(method, new Uri(new Uri(ConnectClient.TokenEndPoint), relativeUri));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.Base64Encode($"{this._clientId}:{this._clientSecret}"));
            httpRequest.Content = content;

            return httpRequest;
        }

        private string Base64Encode(string value) {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}