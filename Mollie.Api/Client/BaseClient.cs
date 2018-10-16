using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Exceptions;
using Mollie.Api.Extensions;

namespace Mollie.Api.Client
{
    public abstract class BaseClient
    {
        private readonly HttpClient _client;
        private readonly string _signaturePrefix;
        private readonly string _apiEndPoint;

        private readonly string _apiVersion;

        private readonly MollieResellerConfigurationOptions _options;

        private HttpClient CreateClient => new HttpClient { BaseAddress = GetBaseAddress() };

        protected BaseClient(string apiEndPoint, string apiVersion, MollieResellerConfigurationOptions options, string signaturePrefix)
        {
            _apiEndPoint = $"{apiEndPoint}{signaturePrefix}";
            _apiVersion = apiVersion;

            _options = options;
            _signaturePrefix = signaturePrefix;

            _client = CreateClient;
        }

        internal static T ProcessHttpResponseMessage<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result.Deserialize<T>();
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.UnsupportedMediaType:
                case (HttpStatusCode)422:
                    throw new ResellerApiException(response.Content.ReadAsStringAsync().Result);

                default:
                    var internalMessage = response?.Content?.ReadAsStringAsync().Result;

                    throw new HttpRequestException(
                        $"Unknown http exception occured with status code: {(int)response.StatusCode}. message: {internalMessage}");
            }
        }

        internal async Task<T> PostAsync<T>(string command, object data)
        {
            var parameters = data.ToDictionary();

            var query = parameters.ToSortedQueryString();

            var signature = $"{_signaturePrefix}/{_apiVersion}/{command}?{query}"
                .Replace("%20", "+")
                .Hash(_options.ResellerSecret);

            parameters.Add("signature", signature);
            var response = await _client.PostAsync(command, new FormUrlEncodedContent(parameters));

            return ProcessHttpResponseMessage<T>(response);
        }

        private Uri GetBaseAddress() => new Uri($"{_apiEndPoint}/{_apiVersion}/");
    }
}
