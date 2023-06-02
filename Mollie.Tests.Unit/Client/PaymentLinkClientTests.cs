using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using RichardSzalay.MockHttp;

/*
namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class PaymentLinkClientTests : BaseClientTests {
        private const decimal DefaultPaymentAmount = 50;
        private const string DefaultPaymentLinkId = "pl_4Y0eZitmBnQ6IDoMqZQKh";
        private const string DefaultDescription = "A car";
        private const string DefaultWebhookUrl = "http://www.mollie.com";
        private const string DefaultRedirectUrl = "http://www.mollie.com";
        
        private readonly string defaultPaymentLinkJsonResponse = @$"{{
    ""resource"": ""payment-link"",
    ""id"": ""{DefaultPaymentLinkId}"",
    ""mode"": ""test"",
    ""profileId"": ""pfl_QkEhN94Ba"",
    ""createdAt"": ""2021-03-20T09:13:37+00:00"",
    ""paidAt"": null,
    ""updatedAt"": null,
    ""expiresAt"": ""2021-06-06T11:00:00+00:00"",
    ""amount"": {{
        ""value"": ""{DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture)}"",
        ""currency"": ""EUR""
    }},
    ""description"": ""{DefaultDescription}"",
    ""redirectUrl"": ""{DefaultRedirectUrl}"",
    ""webhookUrl"": ""{DefaultWebhookUrl}"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/payment-links/pl_4Y0eZitmBnQ6IDoMqZQKh"",
            ""type"": ""application/json""
        }},
        ""paymentLink"": {{
            ""href"": ""https://useplink.com/payment/4Y0eZitmBnQ6IDoMqZQKh/"",
            ""type"": ""text/html""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/payment-links-api/create-payment-link"",
            ""type"": ""text/html""
        }}
    }}
}}";

        [Test]
        public async Task CreatePaymentLinkAsync_PaymentLinkWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
            // Given: we create a payment link request with only the required parameters
            PaymentLinkRequest paymentLinkRequest = new PaymentLinkRequest() {
                Description = "Test",
                Amount = new Amount(Currency.EUR, 50),
                WebhookUrl = "http://www.mollie.com",
                RedirectUrl = "http://www.mollie.com",
                ExpiresAt = DateTime.Now.AddDays(1)
            };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .Respond("application/json", defaultPaymentLinkJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("abcde", httpClient);
            
            // When: We send the request
            PaymentLinkResponse response = await paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            VerifyPaymentLinkResponse(response);
        }

        [Test]
        public async Task GetPaymentLinkAsync_ResponseIsDeserializedInExpectedFormat() {
            // Given: we retrieve a payment link
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .Respond("application/json", defaultPaymentLinkJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("abcde", httpClient);
            
            // When: We send the request
            PaymentLinkResponse response = await paymentLinkClient.GetPaymentLinkAsync(DefaultPaymentLinkId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            VerifyPaymentLinkResponse(response);
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetPaymentLinkAsync_NoPaymentLinkIdIsGiven_ArgumentExceptionIsThrown(string paymentLinkId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await paymentLinkClient.GetPaymentLinkAsync(paymentLinkId));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentLinkId' is null or empty", exception.Message); 
        }

        private void VerifyPaymentLinkResponse(PaymentLinkResponse response) {
            Assert.AreEqual(DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture), response.Amount.Value);
            Assert.AreEqual(DefaultDescription, response.Description);
            Assert.AreEqual(DefaultPaymentLinkId, response.Id);
            Assert.AreEqual(DefaultRedirectUrl, response.RedirectUrl);
            Assert.AreEqual(DefaultWebhookUrl, response.WebhookUrl);
        }
    }
}*/