using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.ContractResolvers;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Connect;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class ConnectClient : IConnectClient {
        private readonly string _clientId;
        private readonly HttpClient _httpClient;

        public ConnectClient(string clientId, string clientSecret) {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException(nameof(clientId));

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException(nameof(clientSecret));

            _clientId = clientId;
            _httpClient = CreateHttpClient(clientId, clientSecret);
        }

        public string GetAuthorizationUrl(string state, List<string> scopes, string redirectUri = null,
            bool forceApprovalPrompt = false) {
            var parameters = new Dictionary<string, string> {
                {"client_id", _clientId},
                {"state", state},
                {"scope", string.Join(" ", scopes)},
                {"response_type", "code"},
                {"approval_prompt", forceApprovalPrompt ? "force" : "auto"}
            };

            if (!string.IsNullOrWhiteSpace(redirectUri))
                parameters.Add("redirect_uri", redirectUri);

            return "https://www.mollie.com/oauth2/authorize?" + string.Join("&",
                       parameters.Select(p => string.Format(
                           $"{WebUtility.UrlEncode(p.Key)}={WebUtility.UrlEncode(p.Value)}")));
        }

        public async Task<TokenResponse> GetAccessTokenAsync(TokenRequest request) {
            var jsonData = JsonConvertExtensions.SerializeObjectSnakeCase(request);
            var response = await _httpClient.PostAsync("https://api.mollie.nl/oauth2/tokens",
                new StringContent(jsonData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TokenResponse>(resultContent,
                new JsonSerializerSettings {ContractResolver = new SnakeCasePropertyNamesContractResolver()});
        }

        /// <summary>
        ///     Creates a new rest client for the Mollie API with basic authentication
        /// </summary>
        private HttpClient CreateHttpClient(string clientId, string clientSecret) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Base64Encode($"{clientId}:{clientSecret}"));
            return httpClient;
        }

        private string Base64Encode(string value) {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}