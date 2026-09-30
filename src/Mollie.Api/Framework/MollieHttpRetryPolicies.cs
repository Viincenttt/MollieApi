using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Mollie.Api.Framework {
    public static class MollieHttpRetryPolicies {
        /// <summary>
        /// Retry policy to automatically retry transient errors.
        /// Transient errors are network failures, http 5xx and http 408 errors.
        /// </summary>
        public static Action<ResiliencePipelineBuilder<HttpResponseMessage>> TransientHttpErrorRetryPolicy(int numberOfRetries = 3) {
            return builder => {
                builder.AddRetry(new HttpRetryStrategyOptions {
                    MaxRetryAttempts = numberOfRetries,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(1),
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(response =>
                            response.StatusCode == HttpStatusCode.RequestTimeout ||
                            (int)response.StatusCode >= 500)
                });
            };
        }
    }
}
