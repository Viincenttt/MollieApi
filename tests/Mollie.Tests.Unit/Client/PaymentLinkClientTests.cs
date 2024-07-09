﻿using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class PaymentLinkClientTests : BaseClientTests {
        private const decimal DefaultPaymentAmount = 50;
        private const string DefaultPaymentLinkId = "pl_4Y0eZitmBnQ6IDoMqZQKh";
        private const string DefaultDescription = "A car";
        private const string DefaultWebhookUrl = "https://www.mollie.com";

        [Fact]
        public async Task CreatePaymentLinkAsync_PaymentLinkWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
            // Given: we create a payment link request with only the required parameters
            PaymentLinkRequest paymentLinkRequest = new PaymentLinkRequest() {
                Description = "Test",
                Amount = new Amount(Currency.EUR, 50),
                WebhookUrl = "https://www.mollie.com",
                RedirectUrl = "https://www.mollie.com",
                ExpiresAt = DateTime.Now.AddDays(1)
            };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .Respond("application/json", _defaultPaymentLinkJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("api-key", httpClient);

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
                .Respond("application/json", _defaultPaymentLinkJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentLinkClient paymentLinkClient = new PaymentLinkClient("api-key", httpClient);

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

        [Theory]
        [InlineData(null, null,  false, null, "")]
        [InlineData("from", null,  false, null, "?from=from")]
        [InlineData("from", 50,  false, null, "?from=from&limit=50")]
        [InlineData(null, null,  true, null, "?testmode=true")]
        [InlineData(null, null,  true, SortDirection.Desc, "?testmode=true&sort=desc")]
        [InlineData(null, null,  true, SortDirection.Asc, "?testmode=true&sort=asc")]
        public async Task GetPaymentLinkPaymentListAsync_QueryParameterOptions_CorrectParametersAreAdded(
            string from,
            int? limit,
            bool testmode,
            SortDirection? sortDirection,
            string expectedQueryString) {
            // Given: We make a request to retrieve the list of orders
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.ApiEndPoint}payment-links/{DefaultPaymentLinkId}/payments{expectedQueryString}",
                _defaultPaymentLinkPaymentsJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var paymentLinkClient = new PaymentLinkClient("access_abcde", httpClient);

            // When: We send the request
            await paymentLinkClient.GetPaymentLinkPaymentListAsync(DefaultPaymentLinkId, from, limit, testmode, sortDirection);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task GetPaymentLinkPaymentListAsync_ResponseIsDeserializedInExpectedFormat() {
            // Given: We make a request to retrieve the list of orders
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.ApiEndPoint}payment-links/{DefaultPaymentLinkId}/payments",
                _defaultPaymentLinkPaymentsJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var paymentLinkClient = new PaymentLinkClient("abcde", httpClient);

            // When: We send the request
            ListResponse<PaymentResponse> result = await paymentLinkClient.GetPaymentLinkPaymentListAsync(DefaultPaymentLinkId);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            PaymentResponse payment = result.Items.Single();
            payment.Id.Should().Be("tr_7UhSN1zuXS");
            payment.Amount.Value.Should().Be(DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture));
            payment.Description.Should().Be(DefaultDescription);
            payment.RedirectUrl.Should().Be(DefaultRedirectUrl);
            payment.WebhookUrl.Should().Be(DefaultWebhookUrl);
        }

        private void VerifyPaymentLinkResponse(PaymentLinkResponse response) {
            response.Amount.Value.Should().Be(DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture));
            response.Description.Should().Be(DefaultDescription);
            response.Id.Should().Be(DefaultPaymentLinkId);
            response.RedirectUrl.Should().Be(DefaultRedirectUrl);
            response.WebhookUrl.Should().Be(DefaultWebhookUrl);
        }

        private readonly string _defaultPaymentLinkPaymentsJsonResponse = $@"{{
  ""count"": 1,
  ""_embedded"": {{
    ""payments"": [
      {{
        ""resource"": ""payment"",
        ""id"": ""tr_7UhSN1zuXS"",
        ""mode"": ""live"",
        ""status"": ""open"",
        ""isCancelable"": false,
        ""amount"": {{
          ""value"": ""{DefaultPaymentAmount.ToString(CultureInfo.InvariantCulture)}"",
          ""currency"": ""EUR""
        }},
        ""description"": ""{DefaultDescription}"",
        ""method"": ""ideal"",
        ""metadata"": null,
        ""details"": null,
        ""profileId"": ""pfl_QkEhN94Ba"",
        ""redirectUrl"": ""{DefaultRedirectUrl}"",
        ""webhookUrl"": ""{DefaultWebhookUrl}"",
        ""createdAt"": ""2024-02-12T11:58:35.0Z"",
        ""expiresAt"": ""2024-02-12T12:13:35.0Z"",
        ""_links"": {{
          ""self"": {{
            ""href"": ""..."",
            ""type"": ""application/hal+json""
          }},
          ""checkout"": {{
            ""href"": ""https://www.mollie.com/checkout/issuer/select/ideal/7UhSN1zuXS"",
            ""type"": ""text/html""
          }},
          ""dashboard"": {{
            ""href"": ""https://www.mollie.com/dashboard/org_12345678/payments/tr_7UhSN1zuXS"",
            ""type"": ""text/html""
          }}
        }}
      }}
    ]
  }},
  ""_links"": {{
    ""previous"": null,
    ""next"": {{
      ""href"": ""https://api.mollie.com/v2/payment-links/pl_4Y0eZitmBnQ6IDoMqZQKh/payments?from=tr_SDkzMggpvx&limit=5"",
      ""type"": ""application/hal+json""
    }}
  }}
}}";

        private readonly string _defaultPaymentLinkJsonResponse = @$"{{
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
    }
}
