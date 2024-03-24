using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework;
using Mollie.Api.Framework.Idempotency;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient : IDisposable {
        public const string ApiEndPoint = "https://api.mollie.com/v2/";
        private readonly string _apiEndpoint = ApiEndPoint;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly JsonConverterService _jsonConverterService;
        
        private readonly AsyncLocalVariable<string> _idempotencyKey = new AsyncLocalVariable<string>(null);

        private readonly bool _createdHttpClient = default;

        protected BaseMollieClient(string apiKey, HttpClient? httpClient = null) {
            if (string.IsNullOrWhiteSpace(apiKey)) {
                throw new ArgumentNullException(nameof(apiKey), "Mollie API key cannot be empty");
            }

            this._jsonConverterService = new JsonConverterService();
            this._createdHttpClient = httpClient == null;
            this._httpClient = httpClient ?? new HttpClient();
            this._apiKey = apiKey;
        }

        protected BaseMollieClient(HttpClient? httpClient = null, string apiEndpoint = ApiEndPoint) {
            this._apiEndpoint = apiEndpoint;
            this._jsonConverterService = new JsonConverterService();
            this._createdHttpClient = httpClient == null;
            this._httpClient = httpClient ?? new HttpClient();
        }
        
        public IDisposable WithIdempotencyKey(string value) {
            _idempotencyKey.Value = value;
            return _idempotencyKey;
        }

        private async Task<T> SendHttpRequest<T>(HttpMethod httpMethod, string relativeUri, object? data = null) {
            HttpRequestMessage httpRequest = this.CreateHttpRequest(httpMethod, relativeUri);
            if (data != null) {
                var jsonData = this._jsonConverterService.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
            }

            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, string? from, int? limit, IDictionary<string, string>? otherParameters = null) {
            string url = relativeUri + this.BuildListQueryString(from, limit, otherParameters);
            return await this.SendHttpRequest<T>(HttpMethod.Get, url).ConfigureAwait(false);
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            return await this.SendHttpRequest<T>(HttpMethod.Get, relativeUri).ConfigureAwait(false);
        }

        protected async Task<T> GetAsync<T>(UrlObjectLink<T> urlObject) {
            this.ValidateUrlLink(urlObject);
            return await this.GetAsync<T>(urlObject.Href).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object? data) {
            return await this.SendHttpRequest<T>(HttpMethod.Post, relativeUri, data).ConfigureAwait(false);
        }

        protected async Task<T> PatchAsync<T>(string relativeUri, object? data) {
            return await this.SendHttpRequest<T>(new HttpMethod("PATCH"), relativeUri, data).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri, object? data = null) {
            await this.SendHttpRequest<object>(HttpMethod.Delete, relativeUri, data).ConfigureAwait(false);
        }

        private async Task<T> ProcessHttpResponseMessage<T>(HttpResponseMessage response) {
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                return this._jsonConverterService.Deserialize<T>(resultContent);
            }

            switch (response.StatusCode) {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.Gone:
                case (HttpStatusCode) 422: // Unprocessable entity
                    throw new MollieApiException(resultContent);
                default:
                    throw new HttpRequestException(
                        $"Unknown http exception occured with status code: {(int) response.StatusCode}.");
            }
        }

        protected void ValidateApiKeyIsOauthAccesstoken(bool isConstructor = false) {
            if (!this._apiKey.StartsWith("access_")) {
                if (isConstructor) {
                    throw new InvalidOperationException(
                        "The provided token isn't an oauth token. You have invoked the method with oauth parameters thus an oauth accesstoken is required.");
                }

                throw new ArgumentException("The provided token isn't an oauth token.");
            }
        }

        private void ValidateUrlLink(UrlLink urlObject) {
            // Make sure the URL is not empty
            if (String.IsNullOrEmpty(urlObject.Href)) {
                throw new ArgumentException($"Url object is null or href is empty: {urlObject}");
            }

            // Don't execute any requests that don't point to the Mollie API URL for security reasons
            if (!urlObject.Href.Contains(this._apiEndpoint)) {
                throw new ArgumentException($"Url does not point to the Mollie API: {urlObject.Href}");
            }
        }

        protected virtual HttpRequestMessage CreateHttpRequest(HttpMethod method, string relativeUri, HttpContent? content = null) {
            HttpRequestMessage httpRequest = new HttpRequestMessage(method, new Uri(new Uri(this._apiEndpoint), relativeUri));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this._apiKey);
            httpRequest.Headers.Add("User-Agent", this.GetUserAgent());
            var idemPotencyKey = _idempotencyKey.Value ?? Guid.NewGuid().ToString();
            httpRequest.Headers.Add("Idempotency-Key", idemPotencyKey);
            httpRequest.Content = content;

            return httpRequest;
        }

        private string BuildListQueryString(string? from, int? limit, IDictionary<string, string>? otherParameters = null) {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.AddValueIfNotNullOrEmpty(nameof(from), from);
            queryParameters.AddValueIfNotNullOrEmpty(nameof(limit), Convert.ToString(limit));

            if (otherParameters != null) {
                foreach (var parameter in otherParameters) {
                    queryParameters.AddValueIfNotNullOrEmpty(parameter.Key, parameter.Value);
                }
            }

            return queryParameters.ToQueryString();
        }

        private string GetUserAgent() {
            const string packageName = "Mollie.Api.NET";
            string versionNumber = typeof(BaseMollieClient).GetTypeInfo().Assembly.GetName().Version.ToString();
            return $"{packageName}/{versionNumber}";
        }
        
        protected void ValidateRequiredUrlParameter(string parameterName, string parameterValue) {
            if (string.IsNullOrWhiteSpace(parameterValue)) {
                throw new ArgumentException($"Required URL argument '{parameterName}' is null or empty");
            }
        }

        public void Dispose()
        {
            if (this._createdHttpClient) {
                _httpClient.Dispose();
            }
        }
    }
}