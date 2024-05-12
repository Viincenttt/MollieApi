using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Options;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";
        protected readonly MollieOptions Configuration = ConfigurationFactory.GetConfiguration().GetSection("Mollie").Get<MollieOptions>();
        protected string ApiKey => this.Configuration.ApiKey;
        protected string ClientId => this.Configuration.ClientId ?? "client-id";
        protected string ClientSecret => this.Configuration.ClientSecret ?? "client-secret";

        protected BaseMollieApiTestClass() {
            this.EnsureTestApiKey(this.ApiKey);

            // Mollie returns a 429 response code (Too many requests) if we send a lot of requests in a short timespan.
            // In order to avoid hitting their rate limit, we add a small delay between each tests.
            TimeSpan timeBetweenTests = TimeSpan.FromMilliseconds(1500);
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
            return string.Compare(json1, json2, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0;
        }
    }
}
