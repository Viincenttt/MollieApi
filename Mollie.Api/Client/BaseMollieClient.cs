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
using System.Linq;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient {
        public const string ApiEndPoint = "https://api.mollie.nl/v2/";

        private readonly string _apiKey;
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;
        private readonly HttpClient _httpClient;

        protected BaseMollieClient(string apiKey) {
            if (string.IsNullOrWhiteSpace(apiKey)) {
                throw new ArgumentNullException(nameof(apiKey), "Mollie API key cannot be empty");
            }

            this._apiKey = apiKey;
            this._defaultJsonDeserializerSettings = this.CreateDefaultJsonDeserializerSettings();
            this._httpClient = this.CreateHttpClient();
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            var response = await this._httpClient.GetAsync(relativeUri).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        private string BuildListQueryString(string from, int? limit, IDictionary<string, string> otherParameters = null) {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(from)) {
                queryParameters[nameof(from)] = from;
            }

            if (limit.HasValue) {
                queryParameters[nameof(limit)] = limit.Value.ToString();
            }
            
            if (otherParameters != null) {
                foreach (var parameter in otherParameters) {
                    queryParameters[parameter.Key] = parameter.Value;
                }
            }

            return queryParameters.ToQueryString();
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, string from, int? limit, IDictionary<string, string> otherParameters = null) {
            var queryString = this.BuildListQueryString(from, limit, otherParameters);
            var response = await this._httpClient.GetAsync(relativeUri + queryString).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object data) {
            var jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
            var response = await this._httpClient
                .PostAsync(relativeUri, new StringContent(jsonData, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri) {
            var response = await this._httpClient.DeleteAsync(relativeUri).ConfigureAwait(false);
            await this.ProcessHttpResponseMessage<object>(response).ConfigureAwait(false);
        }

        private async Task<T> ProcessHttpResponseMessage<T>(HttpResponseMessage response) {
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<T>(resultContent, this._defaultJsonDeserializerSettings);
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

        protected void ValidateApiKeyIsOauthAccesstoken(bool isConstructor = false) {
            if (!this._apiKey.StartsWith("access_")) {
                if (isConstructor) {
                    throw new InvalidOperationException(
                        "The provided token isn't an oauth token. You have invoked the method with oauth parameters thus an oauth accesstoken is required.");
                }
                throw new ArgumentException("The provided token isn't an oauth token.");
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._apiKey);

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