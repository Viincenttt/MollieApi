using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework;
using Mollie.Api.Framework.Authentication;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Framework.Idempotency;
using Mollie.Api.Models.Error;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient : IBaseMollieClient {
        public const string DefaultBaseApiEndPoint = "https://api.mollie.com/v2/";

        private readonly string _apiEndpoint = DefaultBaseApiEndPoint;
        private readonly IMollieSecretManager _mollieSecretManager;
        private readonly MollieClientOptions _options;
        private readonly HttpClient _httpClient;
        private readonly JsonConverterService _jsonConverterService;

        private readonly AsyncLocalVariable<string> _idempotencyKey = new (null);

        private readonly bool _createdHttpClient;

        protected BaseMollieClient(string apiKey, HttpClient? httpClient = null) {
            if (string.IsNullOrWhiteSpace(apiKey)) {
                throw new ArgumentNullException(nameof(apiKey), "Mollie API key cannot be empty");
            }

            _jsonConverterService = new JsonConverterService();
            _createdHttpClient = httpClient == null;
            _httpClient = httpClient ?? new HttpClient();
            _mollieSecretManager = new DefaultMollieSecretManager(apiKey);
            _options = new MollieClientOptions {
                ApiKey = apiKey
            };
        }

        protected BaseMollieClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) {
            _jsonConverterService = new JsonConverterService();
            _createdHttpClient = httpClient == null;
            _httpClient = httpClient ?? new HttpClient();
            _mollieSecretManager = mollieSecretManager;
            _options = options;
            _apiEndpoint = options.ApiBaseUrl;
        }

        protected BaseMollieClient(HttpClient? httpClient = null, string apiEndpoint = DefaultBaseApiEndPoint) {
            _apiEndpoint = apiEndpoint;
            _jsonConverterService = new JsonConverterService();
            _createdHttpClient = httpClient == null;
            _httpClient = httpClient ?? new HttpClient();
            _mollieSecretManager = new DefaultMollieSecretManager(string.Empty);
            _options = new() {
                ApiKey = string.Empty
            };
        }

        public IDisposable WithIdempotencyKey(string value) {
            _idempotencyKey.Value = value;
            return _idempotencyKey;
        }

        private async Task<T> SendHttpRequest<T>(
            HttpMethod httpMethod, string relativeUri, object? data = null, CancellationToken cancellationToken = default) {
            HttpRequestMessage httpRequest = CreateHttpRequest(httpMethod, relativeUri);
            if (data != null) {
                if (data is ITestModeRequest testModeRequest) {
                    testModeRequest.Testmode ??= _options.Testmode;
                }

                if (data is IProfileRequest profileRequest) {
                    profileRequest.ProfileId ??= _options.ProfileId;
                }

                var jsonData = _jsonConverterService.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
            }

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            return await ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> GetListAsync<T>(
            string relativeUri, string? from, int? limit, IDictionary<string, string>? otherParameters = null, CancellationToken cancellationToken = default) {
            string url = relativeUri + BuildListQueryString(from, limit, otherParameters);
            return await SendHttpRequest<T>(HttpMethod.Get, url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<T> GetAsync<T>(string relativeUri, CancellationToken cancellationToken = default) {
            return await SendHttpRequest<T>(HttpMethod.Get, relativeUri, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<T> GetAsync<T>(UrlObjectLink<T> urlObject, CancellationToken cancellationToken = default) {
            ValidateUrlLink(urlObject);
            return await GetAsync<T>(urlObject.Href, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(
            string relativeUri, object? data, CancellationToken cancellationToken = default) {
            return await SendHttpRequest<T>(HttpMethod.Post, relativeUri, data, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<T> PatchAsync<T>(string relativeUri, object? data, CancellationToken cancellationToken = default) {
            return await SendHttpRequest<T>(new HttpMethod("PATCH"), relativeUri, data, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri, object? data = null, CancellationToken cancellationToken = default) {
            await SendHttpRequest<object>(HttpMethod.Delete, relativeUri, data, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<T> ProcessHttpResponseMessage<T>(HttpResponseMessage response) {
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                resultContent = string.IsNullOrEmpty(resultContent) ? "{}" : resultContent;
                return _jsonConverterService.Deserialize<T>(resultContent)!;
            }

            MollieErrorMessage errorDetails = ParseMollieErrorMessage(response.StatusCode, resultContent);
            throw new MollieApiException(errorDetails);
        }

        protected void ValidateApiKeyIsOauthAccesstoken(bool isConstructor = false) {
            string apiKey = _mollieSecretManager.GetBearerToken();
            if (!apiKey.StartsWith("access_")) {
                if (isConstructor) {
                    throw new InvalidOperationException(
                        "The provided token isn't an oauth token. Are you trying to use oauth specific clients using an API key?");
                }

                throw new InvalidOperationException(
                    "The provided token isn't an oauth token. Are you trying to use oauth specific parameters such as ProfileId or TestMode using an API key?");
            }
        }

        private void ValidateUrlLink(UrlLink urlObject) {
            // Make sure the URL is not empty
            if (string.IsNullOrEmpty(urlObject.Href)) {
                throw new ArgumentException($"Url object is null or href is empty: {urlObject}");
            }

            // Don't execute any requests that don't point to the Mollie API URL for security reasons
            if (!urlObject.Href.Contains(_apiEndpoint)) {
                throw new ArgumentException($"Url does not point to the Mollie API: {urlObject.Href}");
            }
        }

        protected virtual HttpRequestMessage CreateHttpRequest(HttpMethod method, string relativeUri, HttpContent? content = null) {
            var httpRequest = new HttpRequestMessage(method, new Uri(new Uri(_apiEndpoint), relativeUri));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _mollieSecretManager.GetBearerToken());
            httpRequest.Headers.Add("User-Agent", GetUserAgent());
            var idemPotencyKey = _idempotencyKey.Value ?? Guid.NewGuid().ToString();
            httpRequest.Headers.Add("Idempotency-Key", idemPotencyKey);
            httpRequest.Content = content;

            return httpRequest;
        }

        private string BuildListQueryString(string? from, int? limit, IDictionary<string, string>? otherParameters = null) {
            var queryParameters = new Dictionary<string, string>();
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
            string userAgent = $"{packageName}/{versionNumber}";

            if (!string.IsNullOrEmpty(_options.CustomUserAgent)) {
                userAgent = $"{userAgent} {_options.CustomUserAgent}";
            }

            return userAgent;
        }

        private MollieErrorMessage ParseMollieErrorMessage(HttpStatusCode responseStatusCode, string responseBody) {
            try {
                return _jsonConverterService.Deserialize<MollieErrorMessage>(responseBody)!;
            }
            catch (JsonException) {
                return new MollieErrorMessage {
                    Title = "Unknown error",
                    Status = (int)responseStatusCode,
                    Detail = responseBody
                };
            }
        }

        protected void ValidateRequiredUrlParameter(string parameterName, string parameterValue) {
            if (string.IsNullOrWhiteSpace(parameterValue)) {
                throw new ArgumentException($"Required URL argument '{parameterName}' is null or empty");
            }
        }

        protected Dictionary<string, string> BuildQueryParameters(
            string? profileId = null, bool testmode = false, SortDirection? sort = null) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode || _options.Testmode == true);
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId ?? _options.ProfileId);
            result.AddValueIfNotNullOrEmpty(nameof(sort), sort?.ToString()?.ToLowerInvariant());
            return result;
        }

        protected TestmodeModel? CreateTestmodeModel(bool testmode) {
            return TestmodeModel.Create(testmode || _options.Testmode == true);
        }

        public void Dispose() {
            if (_createdHttpClient) {
                _httpClient.Dispose();
            }
        }
    }
}
