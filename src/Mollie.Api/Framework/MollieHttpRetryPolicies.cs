using System;
using System.Net;
using System.Net.Http;
#if NET8_0_OR_GREATER
using Microsoft.Extensions.Http.Resilience;
#endif
using Polly;
#if !NET8_0_OR_GREATER
using Polly.Retry;
#endif

namespace Mollie.Api.Framework {
    public static class MollieHttpRetryPolicies {
        /// <summary>
        /// Retry policy to automatically retry transient errors.
        /// Transient errors are network failures, http 5xx and http 408 errors.
        /// </summary>
        public static Action<ResiliencePipelineBuilder<HttpResponseMessage>> TransientHttpErrorRetryPolicy(int numberOfRetries = 3) {
            return builder => {
#if NET8_0_OR_GREATER
                builder.AddRetry(new HttpRetryStrategyOptions {
#else
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage> {
                    UseJitter = true,
#endif
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
