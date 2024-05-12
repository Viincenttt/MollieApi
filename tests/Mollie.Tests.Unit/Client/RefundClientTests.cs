using System;
using Mollie.Api.Client;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Models;
using Mollie.Api.Models.Refund;
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
            var refundResponse = await refundClient.GetRefundAsync("paymentId", "refundId", testmode: testMode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            refundResponse.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);
            var refund = new RefundRequest  {
                Amount = new Amount(Currency.EUR, 100m)
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CreateRefundAsync(paymentId, refund));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
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
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundListAsync(paymentId: paymentId));

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
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundAsync(paymentId, "refund-id"));

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
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundAsync("payment-id", refundId));

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
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelRefundAsync(paymentId, "refund-id"));

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
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelRefundAsync("payment-id", refundId));

            // Then
            exception.Message.Should().Be("Required URL argument 'refundId' is null or empty");
        }
    }
}
