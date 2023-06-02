﻿using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
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

        [Fact]
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

        [Fact]
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
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetPaymentLinkAsync_NoPaymentLinkIdIsGiven_ArgumentExceptionIsThrown(string paymentLinkId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentLinkClient.GetPaymentLinkAsync(paymentLinkId));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentLinkId' is null or empty");
        }

        private void VerifyPaymentLinkResponse(PaymentLinkResponse response) {
            response.Amount.Value.Should().Be(DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture));
            response.Description.Should().Be(DefaultDescription);
            response.Id.Should().Be(DefaultPaymentLinkId);
            response.RedirectUrl.Should().Be(DefaultRedirectUrl);
            response.WebhookUrl.Should().Be(DefaultWebhookUrl);
        }
    }
}