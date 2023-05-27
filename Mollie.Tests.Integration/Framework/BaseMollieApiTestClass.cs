using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        public const int NumberOfRetries = 3;

        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";
        protected readonly MollieIntegationTestConfiguration Configuration = ConfigurationFactory.GetConfiguration().GetSection("Mollie").Get<MollieIntegationTestConfiguration>();
        protected string ApiKey => this.Configuration.ApiKey;
        protected string ClientId => this.Configuration.ClientId ?? "client-id";
        protected string ClientSecret => this.Configuration.ClientSecret ?? "client-secret";
        protected string AccessKey => this.Configuration.AccessKey ?? "access-key";

        protected IPaymentClient _paymentClient;
        protected IPaymentLinkClient _paymentLinkClient;
        protected IPaymentMethodClient _paymentMethodClient;
        protected IRefundClient _refundClient;
        protected ISubscriptionClient _subscriptionClient;
        protected IMandateClient _mandateClient;
        protected ICustomerClient _customerClient;
        protected IProfileClient _profileClient;
        protected IOrderClient _orderClient;
        protected IShipmentClient _shipmentClient;
        protected IBalanceClient _balanceClient;
        protected ITerminalClient _terminalClient;
        protected ICaptureClient _captureClient;

        [OneTimeSetUp]
        public void InitClass() {
            this.EnsureTestApiKey(this.ApiKey);

            this._paymentClient = new PaymentClient(this.ApiKey);
            this._paymentLinkClient = new PaymentLinkClient(this.ApiKey);
            this._paymentMethodClient = new PaymentMethodClient(this.ApiKey);
            this._refundClient = new RefundClient(this.ApiKey);
            this._subscriptionClient = new SubscriptionClient(this.ApiKey);
            this._mandateClient = new MandateClient(this.ApiKey);
            this._customerClient = new CustomerClient(this.ApiKey);
            this._profileClient = new ProfileClient(this.ApiKey);
            this._orderClient = new OrderClient(this.ApiKey);
            this._shipmentClient = new ShipmentClient(this.ApiKey);
            this._balanceClient = new BalanceClient(this.AccessKey);
            this._terminalClient = new TerminalClient(this.ApiKey);
            this._captureClient = new CaptureClient(this.ApiKey);
        }

        [SetUp]
        public void Init() {
            // Mollie returns a 429 response code (Too many requests) if we send a lot of requests in a short timespan. 
            // In order to avoid hitting their rate limit, we add a small delay between each tests. 
            TimeSpan timeBetweenTests = TimeSpan.FromMilliseconds(1000);
            Thread.Sleep(timeBetweenTests);
        }        

        private void EnsureTestApiKey(string apiKey) {
            if (String.IsNullOrEmpty(apiKey)) {
                throw new ArgumentException("Please enter you API key in the BaseMollieApiTestClass class");
            }

            if (!apiKey.StartsWith("test")) {
                throw new ArgumentException("You should not run these tests on your live key!");
            }
        }

        protected bool IsJsonResultEqual(string json1, string json2) {
            return String.Compare(json1, json2, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0;
        }
    }
}
