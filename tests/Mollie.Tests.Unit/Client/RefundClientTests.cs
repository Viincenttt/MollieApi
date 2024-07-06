using System;
using System.Collections.Generic;
using Mollie.Api.Client;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Refund.Request;
using Mollie.Api.Models.Refund.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class RefundClientTests : BaseClientTests {
        private const string defaultSettlementId = "stl_BkEjN2eBb";
        private readonly string defaultGetRefundResponse = @$"{{
    ""resource"": ""refund"",
    ""id"": ""re_4qqhO89gsT"",
    ""amount"": {{
        ""currency"": ""EUR"",
        ""value"": ""5.95""
    }},
    ""settlementId"": ""{defaultSettlementId}"",
    ""status"": ""pending"",
    ""createdAt"": ""2018-03-14T17:09:02.0Z"",
    ""description"": ""Order #33"",
    ""metadata"": {{
         ""bookkeeping_id"": 12345
    }},
    ""paymentId"": ""tr_WDqYK6vllg"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg/refunds/re_4qqhO89gsT"",
            ""type"": ""application/hal+json""
        }},
        ""payment"": {{
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/refunds-api/get-refund"",
            ""type"": ""text/html""
        }}
    }}
}}";

        [Theory]
        [InlineData("payments/paymentId/refunds/refundId", null)]
        [InlineData("payments/paymentId/refunds/refundId", false)]
        [InlineData("payments/paymentId/refunds/refundId?testmode=true", true)]
        public async Task GetRefundAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool? testModeParameter) {
            // Given: We make a request to retrieve a payment without wanting any extra data
            bool testMode = testModeParameter ?? false;
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}{expectedUrl}", defaultGetRefundResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("abcde", httpClient);

            // When: We send the request
            var refundResponse = await refundClient.GetPaymentRefundAsync("paymentId", "refundId", testmode: testMode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            refundResponse.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Given: We create a refund without specifying a paymentId
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);
            var refund = new RefundRequest  {
                Amount = new Amount(Currency.EUR, 100m)
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CreatePaymentRefundAsync(paymentId, refund));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateRefundAsync_WithReverseRouting_ResponseIsDeserializedInExpectedFormat(bool reverseRouting) {
            // Given: We create a refund with a routing destination
            const string paymentId = "tr_7UhSN1zuXS";
            var refundRequest = new RefundRequest  {
                Amount = new Amount(Currency.EUR, 100m),
                ReverseRouting = reverseRouting
            };
            string expectedStringValue = reverseRouting.ToString().ToLowerInvariant();
            string expectedRoutingInformation = $"\"reverseRouting\": {expectedStringValue}";
            string expectedJsonResponse = @$"{{
  ""resource"": ""refund"",
  ""id"": ""re_4qqhO89gsT"",
  ""description"": """",
  ""amount"": {{
    ""currency"": ""EUR"",
    ""value"": ""100.00""
  }},
  ""reverseRouting"": ""{expectedStringValue}"",
  ""status"": ""pending"",
  ""metadata"": null,
  ""paymentId"": ""{paymentId}"",
  ""createdAt"": ""2023-03-14T17:09:02.0Z""
}}";
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}/refunds",
                expectedJsonResponse,
                expectedRoutingInformation);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new("api-key", httpClient);

            // When: We create the refund
            RefundResponse refundResponse = await refundClient.CreatePaymentRefundAsync(paymentId, refundRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            refundResponse.ReverseRouting.Should().Be(reverseRouting);
            refundResponse.RoutingReversals.Should().BeNull();
        }

        [Fact]
        public async Task CreateRefundAsync_WithRoutingInformation_ResponseIsDeserializedInExpectedFormat() {
            // Given: We create a refund with a routing destination
            const string paymentId = "tr_7UhSN1zuXS";
            var refundRequest = new RefundRequest  {
                Amount = new Amount(Currency.EUR, 100m),
                ReverseRouting = null,
                RoutingReversals = new List<RoutingReversal> {
                    new RoutingReversal {
                        Amount = new Amount(Currency.EUR, 50m),
                        Source = new RoutingDestination {
                            Type = "organization",
                            OrganizationId = "organization-id"
                        }
                    }
                }
            };
            string expectedRoutingInformation = $"\"routingReversals\":[{{\"amount\":{{\"currency\":\"EUR\",\"value\":\"50.00\"}},\"source\":{{\"type\":\"organization\",\"organizationId\":\"organization-id\"}}}}]}}";
            string expectedJsonResponse = @$"{{
  ""resource"": ""refund"",
  ""id"": ""re_4qqhO89gsT"",
  ""description"": """",
  ""amount"": {{
    ""currency"": ""EUR"",
    ""value"": ""100.00""
  }},
  ""routingReversals"": [
    {{
      ""amount"": {{
        ""currency"": ""EUR"",
        ""value"": ""50.00""
      }},
      ""source"": {{
        ""type"": ""organization"",
        ""organizationId"": ""organization-id""
      }}
    }}
  ],
  ""status"": ""pending"",
  ""metadata"": null,
  ""paymentId"": ""{paymentId}"",
  ""createdAt"": ""2023-03-14T17:09:02.0Z""
}}";
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}/refunds",
                expectedJsonResponse,
                expectedRoutingInformation);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new("api-key", httpClient);

            // When: We create the refund
            RefundResponse refundResponse = await refundClient.CreatePaymentRefundAsync(paymentId, refundRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            refundResponse.RoutingReversals.Should().BeEquivalentTo(refundRequest.RoutingReversals);
            refundResponse.ReverseRouting.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetRefundListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetPaymentRefundListAsync(paymentId: paymentId));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetPaymentRefundAsync(paymentId, "refund-id"));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetRefundAsync_NoRefundIsGiven_ArgumentExceptionIsThrown(string refundId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetPaymentRefundAsync("payment-id", refundId));

            // Then
            exception.Message.Should().Be("Required URL argument 'refundId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelPaymentRefundAsync(paymentId, "refund-id"));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelRefundAsync_NoRefundIsGiven_ArgumentExceptionIsThrown(string refundId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelPaymentRefundAsync("payment-id", refundId));

            // Then
            exception.Message.Should().Be("Required URL argument 'refundId' is null or empty");
        }

        [Theory]
        [InlineData(null, null, false, "")]
        [InlineData("from", null, false, "?from=from")]
        [InlineData("from", 50, false, "?from=from&limit=50")]
        [InlineData(null, null, true, "?testmode=true")]
        public async Task GetOrderRefundListAsync_QueryParameterOptions_CorrectParametersAreAdded(string from, int? limit, bool testmode, string expectedQueryString) {
            // Given: We make a request to retrieve the list of orders
            const string orderId = "abcde";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}/refunds{expectedQueryString}", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            await refundClient.GetOrderRefundListAsync(orderId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task CreateOrderRefundAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat()
        {
            // Given: We create a refund request with only the required parameters
            const string orderId = "ord_stTC2WHAuS";
            OrderRefundRequest orderRefundRequest = new OrderRefundRequest()
            {
                Description = "description",
                Lines = new[]
                {
                    new OrderLineDetails()
                    {
                        Id = "odl_dgtxyl",
                        Quantity = 1,
                        Amount = new Amount(Currency.EUR, "399.00")
                    }
                },
                Metadata = "my-metadata"
            };
            string url = $"{BaseMollieClient.ApiEndPoint}orders/{orderId}/refunds";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, url, defaultOrderRefundJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var response = await refundClient.CreateOrderRefundAsync(orderId, orderRefundRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            response.Resource.Should().Be("refund");
            response.Id.Should().Be("re_4qqhO89gsT");
            response.Description.Should().Be("description");
            response.Status.Should().Be("pending");
            response.CreatedAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(14.March(2018).At(17, 09, 02), DateTimeKind.Utc));
            response.PaymentId.Should().Be("tr_WDqYK6vllg");
            response.OrderId.Should().Be(orderId);
            response.Lines.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateOrderRefundAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);
            var request = new OrderRefundRequest()
            {
                Lines = new List<OrderLineDetails>()
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CreateOrderRefundAsync(orderId, request));

            // Then
            exception.Message.Should().Be("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetOrderRefundListAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetOrderRefundListAsync(orderId));

            // Then
            exception.Message.Should().Be("Required URL argument 'orderId' is null or empty");
        }

        private const string defaultOrderJsonResponse = @"{
            ""resource"": ""order"",
            ""id"": ""ord_kEn1PlbGa"",
            ""profileId"": ""pfl_URR55HPMGx"",
            ""method"": ""ideal"",
            ""amount"": {
                ""value"": ""1027.99"",
                ""currency"": ""EUR""
            },
        }";

        private const string defaultOrderRefundJsonResponse = @"{
    ""resource"": ""refund"",
    ""id"": ""re_4qqhO89gsT"",
    ""amount"": {
        ""currency"": ""EUR"",
        ""value"": ""698.00""
    },
    ""status"": ""pending"",
    ""createdAt"": ""2018-03-14T17:09:02.0Z"",
    ""description"": ""description"",
    ""metadata"": {
         ""bookkeeping_id"": 12345
    },
    ""paymentId"": ""tr_WDqYK6vllg"",
    ""orderId"": ""ord_stTC2WHAuS"",
    ""lines"": [
        {
            ""resource"": ""orderline"",
            ""id"": ""odl_dgtxyl"",
            ""orderId"": ""ord_stTC2WHAuS"",
            ""name"": ""LEGO 42083 Bugatti Chiron"",
            ""sku"": ""5702016116977"",
            ""type"": ""physical"",
            ""status"": ""paid"",
            ""metadata"": null,
            ""quantity"": 1,
            ""unitPrice"": {
                ""value"": ""399.00"",
                ""currency"": ""EUR""
            },
            ""vatRate"": ""21.00"",
            ""vatAmount"": {
                ""value"": ""51.89"",
                ""currency"": ""EUR""
            },
            ""discountAmount"": {
                ""value"": ""100.00"",
                ""currency"": ""EUR""
            },
            ""totalAmount"": {
                ""value"": ""299.00"",
                ""currency"": ""EUR""
            },
            ""createdAt"": ""2018-08-02T09:29:56+00:00"",
            ""_links"": {
                ""productUrl"": {
                    ""href"": ""https://shop.lego.com/nl-NL/Bugatti-Chiron-42083"",
                    ""type"": ""text/html""
                },
                ""imageUrl"": {
                    ""href"": ""https://sh-s7-live-s.legocdn.com/is/image//LEGO/42083_alt1?$main$"",
                    ""type"": ""text/html""
                }
            }
        }
    ],
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg/refunds/re_4qqhO89gsT"",
            ""type"": ""application/hal+json""
        },
        ""payment"": {
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
            ""type"": ""application/hal+json""
        },
        ""order"": {
            ""href"": ""https://api.mollie.com/v2/orders/ord_stTC2WHAuS"",
            ""type"": ""application/hal+json""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/refunds-api/create-order-refund"",
            ""type"": ""text/html""
        }
    }
}";
    }
}
