using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    using System.Net.Http.Headers;
    using System.Text;

    public abstract class BaseMollieClient {
        public const string ApiEndPoint = "https://api.mollie.nl";
        public const string ApiVersion = "v1";

        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;

        protected BaseMollieClient(string apiKey) {
            if (string.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Mollie API key cannot be empty");
            }

            this._apiKey = apiKey;
            this._defaultJsonDeserializerSettings = this.CreateDefaultJsonDeserializerSettings();
            this._httpClient = this.CreateHttpClient();
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            HttpResponseMessage response = await this._httpClient.GetAsync(relativeUri).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, int? offset, int? count) {
            string queryString = String.Empty;
            if (offset.HasValue) {
                queryString += $"?offset={offset.Value}";
            }
            if (count.HasValue) {
                string separator = String.IsNullOrEmpty(queryString) ? "?" : "&";
                queryString += $"{separator}count={count.Value}";
            }
            HttpResponseMessage response = await this._httpClient.GetAsync(relativeUri + queryString).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object data) {
            string jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
            HttpResponseMessage response = await this._httpClient.PostAsync(relativeUri, new StringContent(jsonData, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri) {
            HttpResponseMessage response = await this._httpClient.DeleteAsync(relativeUri).ConfigureAwait(false);
            await this.ProcessHttpResponseMessage<object>(response).ConfigureAwait(false);
        }

        private async Task<T> ProcessHttpResponseMessage<T>(HttpResponseMessage response) {
            string resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<T>(resultContent, this._defaultJsonDeserializerSettings);
            }
            else {
                switch (response.StatusCode) {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.UnsupportedMediaType:
                    case (HttpStatusCode)422: // Unprocessable entity
                        throw new MollieApiException(resultContent);
                    default:
                        throw new HttpRequestException($"Unknown http exception occured with status code: {(int)response.StatusCode}.");
                }
            }
        }

        /// <summary>
        /// Creates a new rest client for the Mollie API
        /// </summary>
        private HttpClient CreateHttpClient() {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = this.GetBaseAddress();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._apiKey);

            return httpClient;
        }

        /// <summary>
        /// Returns the base address of the Mollie API
        /// </summary>
        /// <returns></returns>
        private Uri GetBaseAddress() => new Uri(ApiEndPoint + "/" + ApiVersion + "/");

        /// <summary>
        /// Creates the default Json serial settings for the JSON.NET parsing.
        /// </summary>
        /// <returns></returns>
        private JsonSerializerSettings CreateDefaultJsonDeserializerSettings() {
            return new JsonSerializerSettings {
                DateFormatString = "MM-dd-yyyy",
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>() {
                    // Add a special converter for payment responses, because we need to create specific classes based on the payment method
                    new PaymentResponseConverter(new PaymentResponseFactory())
                }
            };
        }
    }
}
