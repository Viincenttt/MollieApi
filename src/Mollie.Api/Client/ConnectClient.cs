using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Connect.Request;
using Mollie.Api.Models.Connect.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class ConnectClient : BaseMollieClient, IConnectClient {
        private const string AuthorizeEndPoint = "https://my.mollie.com/oauth2/authorize";
        private const string TokenEndPoint = "https://api.mollie.com/oauth2/";

        private readonly string _clientId;
        private readonly string _clientSecret;

        public ConnectClient(string? clientId, string? clientSecret, HttpClient? httpClient = null): base(httpClient, ConnectClient.TokenEndPoint) {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret)) {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            _clientSecret = clientSecret!;
            _clientId = clientId!;
        }

        [ActivatorUtilitiesConstructor]
        public ConnectClient(MollieClientOptions options, HttpClient? httpClient = null): base(httpClient, ConnectClient.TokenEndPoint) {
            if (string.IsNullOrWhiteSpace(options.ClientId)) {
                throw new ArgumentNullException(nameof(options.ClientId));
            }

            if (string.IsNullOrWhiteSpace(options.ClientSecret)) {
                throw new ArgumentNullException(nameof(options.ClientSecret));
            }

            _clientSecret = options.ClientSecret!;
            _clientId = options.ClientId!;
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

        public async Task<TokenResponse> GetAccessTokenAsync(
            TokenRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<TokenResponse>(
                "tokens", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task RevokeTokenAsync(
            RevokeTokenRequest request, CancellationToken cancellationToken = default) {
            await DeleteAsync(
                "tokens", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected override HttpRequestMessage CreateHttpRequest(
            HttpMethod method, string relativeUri, HttpContent? content = null) {
            var httpRequest = new HttpRequestMessage(method, new Uri(new Uri(TokenEndPoint), relativeUri));
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
