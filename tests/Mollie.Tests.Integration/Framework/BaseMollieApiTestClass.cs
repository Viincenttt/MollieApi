using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Client;
using Mollie.Api.Options;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";

        private readonly MollieOptions Configuration =
            ConfigurationFactory.GetConfiguration().GetSection("Mollie").Get<MollieOptions>();

        protected string ApiKey => Configuration.ApiKey;
        protected string ClientId => Configuration.ClientId ?? "client-id";
        protected string ClientSecret => Configuration.ClientSecret ?? "client-secret";

        protected BaseMollieApiTestClass() {
            EnsureTestApiKey(ApiKey);

            // Mollie returns a 429 response code (Too many requests) if we send a lot of requests in a short timespan.
            // this is partially mitigated by using a custom http retry policy. However, the RetryAfter header gets
            // exceedingly long if we do too many requests without any delay. This is why we add a small delay between
            // each test.
            TimeSpan timeBetweenTests = TimeSpan.FromMilliseconds(500);
            Thread.Sleep(timeBetweenTests);
        }

        private void EnsureTestApiKey(string apiKey) {
            if (string.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Please enter you API key in the BaseMollieApiTestClass class");
            }

            if (!apiKey.StartsWith("test")) {
                throw new ArgumentException("You should not run these tests on your live key!");
            }
        }

        protected bool IsJsonResultEqual(string json1, string json2) {
            return string.Compare(json1, json2, CultureInfo.CurrentCulture,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0;
        }

        protected async Task<TResult> ExecuteWithRetry<TResult>(Func<Task<TResult>> apiAction, int numberOfRetries = 3) {
            MollieApiException exception = null;
            for (int i = 0; i < numberOfRetries; i++) {
                try {
                    return await apiAction.Invoke();
                } catch (MollieApiException ex) {
                    exception = ex;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }

            throw exception!;
        }

        protected async Task ExecuteWithRetry(Func<Task> apiAction, int numberOfRetries = 3) {
            MollieApiException exception = null;
            for (int i = 0; i < numberOfRetries; i++) {
                try {
                    await apiAction.Invoke();
                } catch (MollieApiException ex) {
                    exception = ex;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }

            throw exception!;
        }
    }
}
