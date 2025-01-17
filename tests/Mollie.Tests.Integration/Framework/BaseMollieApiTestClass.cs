using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Options;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";
        private readonly MollieOptions Configuration = ConfigurationFactory.GetConfiguration().GetSection("Mollie").Get<MollieOptions>();
        protected string ApiKey => Configuration.ApiKey;
        protected string ClientId => Configuration.ClientId ?? "client-id";
        protected string ClientSecret => Configuration.ClientSecret ?? "client-secret";

        protected BaseMollieApiTestClass() {
            EnsureTestApiKey(ApiKey);
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
