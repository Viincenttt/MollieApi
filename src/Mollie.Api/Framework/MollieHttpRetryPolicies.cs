using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace Mollie.Api.Framework {
    public static class MollieHttpRetryPolicies {
        /// <summary>
        /// Retry policy to automatically retry transient errors.
        /// Transient errors are network failures, http 5xx and http 408 errors. When the response contains a
        /// Retry-After header, its value is used as the delay before the next attempt.
        /// </summary>
        public static Action<ResiliencePipelineBuilder<HttpResponseMessage>> TransientHttpErrorRetryPolicy(int numberOfRetries = 3) {
            return builder => {
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage> {
                    MaxRetryAttempts = numberOfRetries,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    Delay = TimeSpan.FromSeconds(1),
                    DelayGenerator = args => new ValueTask<TimeSpan?>(GetRetryAfterDelay(args.Outcome.Result, DateTimeOffset.UtcNow)),
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(response =>
                            response.StatusCode == HttpStatusCode.RequestTimeout ||
                            (int)response.StatusCode >= 500)
                });
            };
        }

        /// <summary>
        /// Returns the delay requested by the Retry-After header of the response, or null when there is none, so the
        /// regular backoff is used.
        /// </summary>
        internal static TimeSpan? GetRetryAfterDelay(HttpResponseMessage? response, DateTimeOffset now) {
            var retryAfter = response?.Headers.RetryAfter;
            if (retryAfter?.Delta is { } delta)
                return delta;

            if (retryAfter?.Date is { } date)
                return date > now ? date - now : TimeSpan.Zero;

            return null;
        }
    }
}
