using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Mollie.Api.Framework {
    public static class MollieHttpRetryPolicies {
        /// <summary>
        /// Retry policy to automatically retry transient errors.
        /// Transient errors are network failures, http 5xx and http 408 errors.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> TransientHttpErrorRetryPolicy(int numberOfRetries = 3) {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(numberOfRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
    }
}