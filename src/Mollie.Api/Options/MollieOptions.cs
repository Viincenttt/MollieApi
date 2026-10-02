using System;
using System.Net.Http;
using System.Text;
using Mollie.Api.Client;
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
        /// The base URL for all API requests. Can be overridden for testing purposes.
        /// </summary>
        public string ApiBaseUrl { get; set; } = BaseMollieClient.DefaultBaseApiEndPoint;

        /// <summary>
        /// The authorize endpoint for the Connect client. Can be overridden for testing purposes.
        /// </summary>
        public string ConnectOAuthAuthorizeEndPoint { get; set; } = ConnectClient.DefaultAuthorizeEndpoint;

        /// <summary>
        /// The token endpoint for the Connect client. Can be overridden for testing purposes.
        /// </summary>
        public string ConnectTokenEndPoint { get; set; } = ConnectClient.DefaultTokenEndpoint;

        /// <summary>
        /// (Optional) Resilience pipeline configuration that is applied to all Mollie API clients.
        /// Requests are not retried unless this property is set. Assign
        /// <see cref="Mollie.Api.Framework.MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy"/> to retry transient
        /// errors, or configure your own retry behaviour for failed requests, e.g.
        /// <c>options.RetryPolicy = builder => builder.AddRetry(new HttpRetryStrategyOptions());</c>
        /// </summary>
        public Action<ResiliencePipelineBuilder<HttpResponseMessage>>? RetryPolicy { get; set; }

        /// <summary>
        /// (Optional) The default user agent is "Mollie.Api.NET {version}". When this property is set, the custom user
        /// agent will be appended to the default user agent.
        /// </summary>
        public string? CustomUserAgent { get; set; }

        /// <summary>
        /// (Optional) Enable test mode for all requests
        /// </summary>
        public bool? Testmode { get; set; }

        /// <summary>
        /// (Optional) The profile ID to be used for all requests
        /// </summary>
        public string? ProfileId { get; set; }

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

        /// <summary>
        /// Prints the options without the values of <see cref="ApiKey"/> and <see cref="ClientSecret"/>, so the
        /// secrets don't end up in logs when the options are logged
        /// </summary>
        protected virtual bool PrintMembers(StringBuilder builder) {
            builder.Append($"{nameof(ApiKey)} = {MaskSecret(ApiKey)}, ");
            builder.Append($"{nameof(ClientId)} = {ClientId}, ");
            builder.Append($"{nameof(ClientSecret)} = {MaskSecret(ClientSecret)}, ");
            builder.Append($"{nameof(ApiBaseUrl)} = {ApiBaseUrl}, ");
            builder.Append($"{nameof(ConnectOAuthAuthorizeEndPoint)} = {ConnectOAuthAuthorizeEndPoint}, ");
            builder.Append($"{nameof(ConnectTokenEndPoint)} = {ConnectTokenEndPoint}, ");
            builder.Append($"{nameof(RetryPolicy)} = {RetryPolicy}, ");
            builder.Append($"{nameof(CustomUserAgent)} = {CustomUserAgent}, ");
            builder.Append($"{nameof(Testmode)} = {Testmode}, ");
            builder.Append($"{nameof(ProfileId)} = {ProfileId}, ");
            builder.Append($"{nameof(CustomMollieSecretManager)} = {CustomMollieSecretManager}");
            return true;
        }

        private static string MaskSecret(string? secret) => string.IsNullOrEmpty(secret) ? string.Empty : "***";
    }
}
