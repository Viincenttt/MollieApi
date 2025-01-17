using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Polly;

namespace Mollie.Tests.Integration.Framework;

public static class MollieIntegrationTestHttpRetryPolicies {

    public static IAsyncPolicy<HttpResponseMessage> TooManyRequestRetryPolicy() {
        var retryPolicy = Policy<HttpResponseMessage>
            .Handle<MollieApiException>(x => x.Details.Status == 429)
            .OrResult(r =>  r?.Headers?.RetryAfter != null)
            .WaitAndRetryAsync(
                3,
                sleepDurationProvider: (_, response, _) =>
                    response.Result.Headers.RetryAfter?.Delta?.Add(TimeSpan.FromSeconds(1)) ?? TimeSpan.FromSeconds(5),
                onRetryAsync: (response, _, _, _) => {
                    // If we send a retry with the same idempotency key, we always get a 429 back...
                    response.Result.RequestMessage?.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
                    return Task.CompletedTask;
                }
            );

        return retryPolicy;
    }
}
