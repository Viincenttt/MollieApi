using System;
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
        /// A custom secret manager that you can override to implement advanced multi-tenant scenario's
        /// </summary>
        public Type? CustomMollieSecretManager { get; private set; }

        /// <summary>
        /// Set a custom secret manager that you can override to implement advanced multi-tenant scenario's
        /// </summary>
        public MollieOptions SetCustomMollieSecretManager<T>() where T : IMollieSecretManager {
            CustomMollieSecretManager = typeof(T);
            return this;
        }
    }
}
