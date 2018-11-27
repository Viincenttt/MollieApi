﻿using System;
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
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public abstract class BaseMollieClient {
        public const string ApiEndPoint = "https://api.mollie.com/v2/";

        private readonly string _apiKey;
        private readonly JsonSerializerSettings _defaultJsonDeserializerSettings;
        private readonly HttpClient _httpClient;

        protected BaseMollieClient(string apiKey, HttpClient httpClient = null) {
            if (string.IsNullOrWhiteSpace(apiKey)) {
                throw new ArgumentNullException(nameof(apiKey), "Mollie API key cannot be empty");
            }

            this._httpClient = httpClient ?? new HttpClient();
            this._apiKey = apiKey;
            this._defaultJsonDeserializerSettings = this.CreateDefaultJsonDeserializerSettings();
        }

        protected async Task<T> GetAsync<T>(string relativeUri) {
            HttpRequestMessage httpRequest = this.CreateHttpRequest(HttpMethod.Get, relativeUri);
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> GetAsync<T>(UrlObjectLink<T> urlObject) {
            this.ValidateUrlLink(urlObject);
            HttpRequestMessage httpRequest = this.CreateHttpRequest(HttpMethod.Get, urlObject.Href);
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);
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
                    if (!String.IsNullOrEmpty(parameter.Value)) {
                        queryParameters[parameter.Key] = parameter.Value;
                    }
                }
            }

            return queryParameters.ToQueryString();
        }

        protected async Task<T> GetListAsync<T>(string relativeUri, string from, int? limit, IDictionary<string, string> otherParameters = null) {
            var queryString = this.BuildListQueryString(from, limit, otherParameters);
            HttpRequestMessage httpRequest = this.CreateHttpRequest(HttpMethod.Get, relativeUri + queryString);
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);
            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> PostAsync<T>(string relativeUri, object data) {
            var jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequest = this.CreateHttpRequest(HttpMethod.Post, relativeUri, content);
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);

            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task<T> PatchAsync<T>(string relativeUri, object data) {
            var jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequest = this.CreateHttpRequest(new HttpMethod("PATCH"), relativeUri, content);
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);

            return await this.ProcessHttpResponseMessage<T>(response).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string relativeUri, object data = null) {
            HttpRequestMessage httpRequest = this.CreateHttpRequest(HttpMethod.Delete, relativeUri);
            if (data != null) {
                var jsonData = JsonConvertExtensions.SerializeObjectCamelCase(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                httpRequest.Content = content;
            }
            var response = await this._httpClient.SendAsync(httpRequest).ConfigureAwait(false);
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

        private void ValidateUrlLink(UrlLink urlObject) {
            // Make sure the URL is not empty
            if (String.IsNullOrEmpty(urlObject?.Href)) {
                throw new ArgumentException($"Url object is null or href is empty: {urlObject}");
            }

            // Don't execute any requests that don't point to the Mollie API URL for security reasons
            if (!urlObject.Href.Contains(ApiEndPoint)) {
                throw new ArgumentException($"Url does not point to the Mollie API: {urlObject.Href}");
            }
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod method, string relativeUri, HttpContent content = null) {
            HttpRequestMessage httpRequest = new HttpRequestMessage(method, new Uri(new Uri(ApiEndPoint), relativeUri));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this._apiKey);
            httpRequest.Content = content;

            return httpRequest;
        }
    }
}