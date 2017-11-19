using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient {
        public const string ApiEndPoint = "https://api.mollie.nl/v1/";

        private readonly string _apiKey;
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;
        private readonly HttpClient _httpClient;

        protected BaseMollieClient(string apiKey) {
            if (string.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Mollie API key cannot be empty");
            }

            _apiKey = apiKey;
            _defaultJsonDeserializerSettings = CreateDefaultJsonDeserializerSettings();
            _httpClient = CreateHttpClient();
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            var response = await _httpClient.GetAsync(relativeUri).ConfigureAwait(false);
            return await ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, int? offset, int? count) {
            var queryString = string.Empty;
            if (offset.HasValue) {
                queryString += $"?offset={offset.Value}";
            }
            if (count.HasValue) {
                var separator = string.IsNullOrEmpty(queryString) ? "?" : "&";
                queryString += $"{separator}count={count.Value}";
            }
            var response = await _httpClient.GetAsync(relativeUri + queryString).ConfigureAwait(false);
            return await ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object data) {
            var jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
            var response = await _httpClient
                .PostAsync(relativeUri, new StringContent(jsonData, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            return await ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri) {
            var response = await _httpClient.DeleteAsync(relativeUri).ConfigureAwait(false);
            await ProcessHttpResponseMessage<object>(response).ConfigureAwait(false);
        }

        private async Task<T> ProcessHttpResponseMessage<T>(HttpResponseMessage response) {
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<T>(resultContent, _defaultJsonDeserializerSettings);
            }
            switch (response.StatusCode) {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.UnsupportedMediaType:
                case (HttpStatusCode) 422: // Unprocessable entity
                    throw new MollieApiException(resultContent);
                default:
                    throw new HttpRequestException(
                        $"Unknown http exception occured with status code: {(int) response.StatusCode}.");
            }
        }

        /// <summary>
        ///     Creates a new rest client for the Mollie API
        /// </summary>
        private HttpClient CreateHttpClient() {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiEndPoint);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            return httpClient;
        }

        /// <summary>
        ///     Creates the default Json serial settings for the JSON.NET parsing.
        /// </summary>
        /// <returns></returns>
        private JsonSerializerSettings CreateDefaultJsonDeserializerSettings() {
            return new JsonSerializerSettings {
                DateFormatString = "MM-dd-yyyy",
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> {
                    // Add a special converter for payment responses, because we need to create specific classes based on the payment method
                    new PaymentResponseConverter(new PaymentResponseFactory())
                }
            };
        }
    }
}