using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    internal class MollieHttpClient {
        public const string ApiEndPoint = "https://api.mollie.nl";
        public const string ApiVersion = "v1";

        private readonly string _apiKey;

        public MollieHttpClient(string apiKey) {
            this._apiKey = apiKey;
        }
        public async Task<T> Get<T>(string relativeUri) {
            Uri absoluteUri = this.GetAbsoluteUri(relativeUri);
            return await this.Get<T>(absoluteUri);
        }

        public async Task<T> Get<T>(Uri absoluteUri) {
            string jsonData = await this.FireGetRequest(absoluteUri);
            T result = JsonConvert.DeserializeObject<T>(jsonData, this.GetDefaultJsonSerializerSettings());

            return result;
        }

        public async Task<ListResponse<T>> GetList<T>(string relativeUri, int? offset, int? count) {
            Uri listUri = this.CreateListRequestUri(relativeUri, offset, count);
            return await this.Get<ListResponse<T>>(listUri);
        }

        public async Task<T> Post<T>(string relativeUri, object data = null) {
            Uri absoluteUri = this.GetAbsoluteUri(relativeUri);
            string jsonResponseData = await this.FirePostRequest(absoluteUri, JsonConvertExtensions.SerializeObjectCamelCase(data));
            T result = JsonConvert.DeserializeObject<T>(jsonResponseData, this.GetDefaultJsonSerializerSettings());

            return result;
        }

        public async Task Delete(string relativeUri) {
            Uri absoluteUri = this.GetAbsoluteUri(relativeUri);
            await this.FireDeleteRequest(absoluteUri);
        }

        /// <summary>
        /// Sends a GET request to the Mollie API and returns the response as a string
        /// </summary>
        private async Task<string> FireGetRequest(Uri uri) {
            HttpClient httpClient = this.CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            return await this.HandleHttpResponseMessage(response);
        }

        /// <summary>
        /// Sends a POST request to the Mollie API and returns the response as a string
        /// </summary>
        private async Task<string> FirePostRequest(Uri uri, string postData) {
            HttpClient httpClient = this.CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(postData));

            return await this.HandleHttpResponseMessage(response);
        }

        /// <summary>
        /// Sends a DELETE request to the mollie API and returns the response as a string
        /// </summary>
        private async Task<string> FireDeleteRequest(Uri uri) {
            HttpClient httpClient = this.CreateHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            return await this.HandleHttpResponseMessage(response);
        }

        private async Task<string> HandleHttpResponseMessage(HttpResponseMessage response) {
            string responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode) {
                  throw new MollieApiException(responseContent);
            }

            return responseContent;
        }

        /// <summary>
        /// Creates a new HttpClient for the mollie api
        /// </summary>
        private HttpClient CreateHttpClient() {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = this.GetBaseAddress();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        /// <summary>
        /// Creates a uri to retrieve a list. Automatically adds the offset and count parameters to the query string
        /// </summary>
        public Uri CreateListRequestUri(string relativeUri, int? offset, int? count) {
            Uri listUri = this.GetAbsoluteUri(relativeUri);
            UriBuilder uriBuilder = new UriBuilder(listUri);
            NameValueCollection queryString = HttpUtility.ParseQueryString(listUri.Query);
            if (offset.HasValue) {
                queryString["offset"] = offset.Value.ToString();
            }
            if (count.HasValue) {
                queryString["count"] = count.Value.ToString();
            }

            uriBuilder.Query = queryString.ToString();
            return new Uri(uriBuilder.ToString());
        }

        private Uri GetAbsoluteUri(string relativeUri) {
            Uri baseAddress = this.GetBaseAddress();
            return new Uri(baseAddress, relativeUri);
        }

        /// <summary>
        /// Returns the base address of the Mollie API
        /// </summary>
        /// <returns></returns>
        private Uri GetBaseAddress() => new Uri(ApiEndPoint + "/" + ApiVersion + "/");

        private JsonSerializerSettings GetDefaultJsonSerializerSettings() => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }
}
