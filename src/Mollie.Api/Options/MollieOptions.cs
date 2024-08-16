using System.Net.Http;
using Mollie.Api.Framework.Authentication.Abstract;
using Polly;

namespace Mollie.Api.Options {
    public record MollieOptions {
        /// <summary>
        /// Your API-key or OAuth token
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// (Optional) ClientId used by Connect API
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// (Optional) ClientSecret used by Connect API
        /// </summary>
        /// <returns></returns>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// (Optional) Polly retry policy for failed requests
        /// </summary>
        public IAsyncPolicy<HttpResponseMessage>? RetryPolicy { get; set; }

        /// <summary>
        /// (Optional) Set a custom bearer token retrieval, in case you have a multi-tenant setup with multiple API keys
        /// </summary>
        public IBearerTokenRetriever? BearerTokenRetriever { get; set; } = null;
    }
}
