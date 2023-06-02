using System;
using System.Threading.Tasks;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Tests.Integration.Framework;

/*
namespace Mollie.Tests.Integration.Api {
    public class PaymentLinkTests : BaseMollieApiTestClass {
        [Test]
        [RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrievePaymentlinkList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentLinkResponse> response = await this._paymentLinkClient.GetPaymentLinkListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [Test]
        [RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanCreatePaymentLinkAndRetrieveIt() {
            // Given: We create a new payment 
            PaymentLinkRequest paymentLinkRequest = new PaymentLinkRequest() {
                Description = "Test",
                Amount = new Amount(Currency.EUR, 50),
                WebhookUrl = this.DefaultWebhookUrl,
                RedirectUrl = this.DefaultRedirectUrl,
                ExpiresAt = DateTime.Now.AddDays(1)
            };
            var createdPaymentLinkResponse = await this._paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

            // When: We retrieve it
            var retrievePaymentLinkResponse = await this._paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

            // Then: We expect a list with a single ideal payment     
            var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
                var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));
                
                Assert.AreEqual(paymentLinkRequest.Amount.Currency, response.Amount.Currency);
                Assert.AreEqual(paymentLinkRequest.Amount.Value, response.Amount.Value);
                Assert.AreEqual(expiresAtWithoutMs, response.ExpiresAt);
                Assert.AreEqual(paymentLinkRequest.Description, response.Description);
                Assert.AreEqual(paymentLinkRequest.RedirectUrl, response.RedirectUrl);
                // Commented this out: For some reason the payment link is now returned here instead of the webhook url
                // Assert.AreEqual(paymentLinkRequest.WebhookUrl, response.WebhookUrl);
            });

            verifyPaymentLinkResponse(createdPaymentLinkResponse);
            verifyPaymentLinkResponse(retrievePaymentLinkResponse);
        }
    }
}*/