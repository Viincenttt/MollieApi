﻿using System;
using System.Threading;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    public abstract class BaseMollieApiTestClass {
        protected readonly string DefaultRedirectUrl = "http://mysite.com";
        protected readonly string DefaultWebhookUrl = "http://mysite.com/webhook";

        protected readonly string ApiTestKey = "test_yGJ4USbh3BWV5AkGbdh4NG4EG2UdaF"; // Insert you API key here

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
            this.EnsureTestApiKey();
            this._paymentClient = new PaymentClient(this.ApiTestKey);
            this._paymentMethodClient = new PaymentMethodClient(this.ApiTestKey);
            this._refundClient = new RefundClient(this.ApiTestKey);
            this._subscriptionClient = new SubscriptionClient(this.ApiTestKey);
            this._mandateClient = new MandateClient(this.ApiTestKey);
            this._customerClient = new CustomerClient(this.ApiTestKey);
            this._profileClient = new ProfileClient(this.ApiTestKey);
            this._orderClient = new OrderClient(this.ApiTestKey);
            this._shipmentClient = new ShipmentClient(this.ApiTestKey);
        }

        [SetUp]
        public void Init() {
            // Mollie returns a 429 response code (Too many requests) if we send a lot of requests in a short timespan. 
            // In order to avoid hitting their rate limit, we add a small delay between each tests. 
            TimeSpan timeBetweenTests = TimeSpan.FromMilliseconds(100);
            Thread.Sleep(timeBetweenTests);
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
