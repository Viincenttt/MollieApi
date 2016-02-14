using System;
using Mollie.Api.Client;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";
        protected readonly string ApiTestKey = ""; // Insert you API key here

        protected MollieApi _mollieClient;

        [OneTimeSetUp]
        public void InitClass() {
            this.EnsureTestApiKey();
            this._mollieClient = new MollieApi(this.ApiTestKey);
        }

        private void EnsureTestApiKey() {
            if (String.IsNullOrEmpty(this.ApiTestKey)) {
                throw new ArgumentException("Please enter you API key on the BaseMollieApiTestClass class");
            }

            if (!this.ApiTestKey.StartsWith("test")) {
                throw new ArgumentException("You should not run these tests on your live key!");
            }
        }
    }
}
