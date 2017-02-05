using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using RestSharp;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient {
        public const string ApiEndPoint = "https://api.mollie.nl";
        public const string ApiVersion = "v1";

        private readonly string _apiKey;
        private readonly RestClient _restClient;
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;

        public BaseMollieClient(string apiKey) {
            if (string.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Mollie API key cannot be empty");
            }

            this._apiKey = apiKey;
            this._defaultJsonDeserializerSettings = this.CreateDefaultJsonDeserializerSettings();
            this._restClient = this.CreateRestClient();
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            RestRequest request = new RestRequest(relativeUri, Method.GET);
            return await this.ExecuteRequestAsync<T>(request).ConfigureAwait(false);
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, int? offset, int? count) {
            RestRequest request = new RestRequest(relativeUri, Method.GET);
            if (offset.HasValue) {
                request.AddParameter("offset", offset);
            }
            if (count.HasValue) {
                request.AddParameter("count", count);
            }

            return await this.ExecuteRequestAsync<T>(request).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object data) {
            RestRequest request = new RestRequest(relativeUri, Method.POST);
            request.AddParameter(String.Empty, JsonConvertExtensions.SerializeObjectCamelCase(data), ParameterType.RequestBody);

            return await this.ExecuteRequestAsync<T>(request).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri) {
            RestRequest request = new RestRequest(relativeUri, Method.DELETE);
            await this.ExecuteRequestAsync<object>(request).ConfigureAwait(false);
        }

        private async Task<T> ExecuteRequestAsync<T>(IRestRequest request) {
            IRestResponse response = await this._restClient.ExecuteTaskAsync(request).ConfigureAwait(false);
            return this.ProcessHttpResponseMessage<T>(response);
        }

        private T ProcessHttpResponseMessage<T>(IRestResponse response) {
            if (response.IsSuccessful()) {
                return JsonConvert.DeserializeObject<T>(response.Content, this._defaultJsonDeserializerSettings);
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
                        throw new MollieApiException(response.Content);
                    default:
                        throw new HttpRequestException($"Unknown http exception occured with status code: {(int)response.StatusCode}.");
                }
            }
        }

        /// <summary>
        /// Creates a new rest client for the Mollie API
        /// </summary>
        private RestClient CreateRestClient() {
            RestClient restClient = new RestClient();
            restClient.BaseUrl = this.GetBaseAddress();
            restClient.AddDefaultHeader("Content-Type", "application/json");
            restClient.AddDefaultParameter("Authorization", $"Bearer {this._apiKey}", ParameterType.HttpHeader);

            return restClient;
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
