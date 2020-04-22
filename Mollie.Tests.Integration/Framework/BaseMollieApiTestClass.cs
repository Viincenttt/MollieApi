using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";

        protected IPaymentClient _paymentClient;
        protected IPaymentMethodClient _paymentMethodClient;
        protected IRefundClient _refundClient;
        protected ISubscriptionClient _subscriptionClient;
        protected IMandateClient _mandateClient;
        protected ICustomerClient _customerClient;
        protected IProfileClient _profileClient;
        protected IOrderClient _orderClient;
        protected IShipmentClient _shipmentClient;

        [OneTimeSetUp]
        public void InitClass() {
            string apiKey = this.GetApiKeyFromConfiguration();
            this.EnsureTestApiKey(apiKey);

            this._paymentClient = new PaymentClient(apiKey);
            this._paymentMethodClient = new PaymentMethodClient(apiKey);
            this._refundClient = new RefundClient(apiKey);
            this._subscriptionClient = new SubscriptionClient(apiKey);
            this._mandateClient = new MandateClient(apiKey);
            this._customerClient = new CustomerClient(apiKey);
            this._profileClient = new ProfileClient(apiKey);
            this._orderClient = new OrderClient(apiKey);
            this._shipmentClient = new ShipmentClient(apiKey);
        }

        [SetUp]
        public void Init() {
            // Mollie returns a 429 response code (Too many requests) if we send a lot of requests in a short timespan. 
            // In order to avoid hitting their rate limit, we add a small delay between each tests. 
            TimeSpan timeBetweenTests = TimeSpan.FromMilliseconds(2000);
            Thread.Sleep(timeBetweenTests);
        }

        protected string GetApiKeyFromConfiguration() {
            IConfiguration configuration = ConfigurationFactory.GetConfiguration();
            MollieConfiguration mollieConfiguration = configuration.GetSection("Mollie").Get<MollieConfiguration>();
            return mollieConfiguration.ApiKey;
        } 

        private void EnsureTestApiKey(string apiKey) {
            if (String.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Please enter you API key in the BaseMollieApiTestClass class");
            }

            if (!apiKey.StartsWith("test")) {
                throw new ArgumentException("You should not run these tests on your live key!");
            }
        }
    }
}
