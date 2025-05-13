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
        public string? ClientId { get; set; } = string.Empty;

        /// <summary>
        /// (Optional) ClientSecret used by Connect API
        /// </summary>
        /// <returns></returns>
        public string? ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// (Optional) Polly retry policy for failed requests
        /// </summary>
        public IAsyncPolicy<HttpResponseMessage>? RetryPolicy { get; set; }

        /// <summary>
        /// (Optional) The default user agent is "Mollie.Api.NET {version}". When this property is set, the custom user
        /// agent will be appended to the default user agent.
        /// </summary>
        public string? CustomUserAgent { get; init; }

        /// <summary>
        /// (Optional) A custom secret manager that you can override to implement advanced multi-tenant scenario's
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
