using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Mollie.Api.Framework {
    public static class MollieHttpRetryPolicies {
        public static IAsyncPolicy<HttpResponseMessage> TransientHttpErrorRetryPolicy(int numberOfRetries = 3) {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(numberOfRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
    }
}