using System;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";

        protected readonly string ApiTestKey = ""; // Insert you API key here

        protected IPaymentClient _paymentClient;
        protected IPaymentMethodClient _paymentMethodClient;
        protected IRefundClient _refundClient;
        protected IIssuerClient _issuerClient;
        protected ISubscriptionClient _subscriptionClient;
        protected IMandateClient _mandateClient;
        protected ICustomerClient _customerClient;

        [OneTimeSetUp]
        public void InitClass() {
            this.EnsureTestApiKey();
            this._paymentClient = new PaymentClient(this.ApiTestKey);
            this._paymentMethodClient = new PaymentMethodClient(this.ApiTestKey);
            this._refundClient = new RefundClient(this.ApiTestKey);
            this._issuerClient = new IssuerClient(this.ApiTestKey);
            this._subscriptionClient = new SubscriptionClient(this.ApiTestKey);
            this._mandateClient = new MandateClient(this.ApiTestKey);
            this._customerClient = new CustomerClient(this.ApiTestKey);
        }

        private void EnsureTestApiKey() {
            if (String.IsNullOrEmpty(this.ApiTestKey)) {
                throw new ArgumentException("Please enter you API key in the BaseMollieApiTestClass class");
            }

            if (!this.ApiTestKey.StartsWith("test")) {
                throw new ArgumentException("You should not run these tests on your live key!");
            }
        }
    }
}
