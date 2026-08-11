using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Mollie.Tests.Integration.Framework;

public static class MollieIntegrationTestHttpRetryPolicies {

    public static Action<ResiliencePipelineBuilder<HttpResponseMessage>> TooManyRequestRetryPolicy() {
        return builder => {
            builder.AddRetry(new HttpRetryStrategyOptions {
                MaxRetryAttempts = 3,
                UseJitter = false,
                BackoffType = DelayBackoffType.Constant,
                Delay = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>().HandleResult(response => response?.Headers?.RetryAfter != null),
                OnRetry = async outcome => {
                    // If we send a retry with the same idempotency key, we always get a 429 back...
                    outcome.Outcome.Result?.RequestMessage?.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
                    await Task.CompletedTask;
                }
            });
        };
    }
}
