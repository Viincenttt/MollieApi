using System;
using Mollie.Api.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Models.Refund;
using RichardSzalay.MockHttp;

/*
namespace Mollie.Tests.Unit.Client {
    [TestFixture]
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

        [TestCase("payments/paymentId/refunds/refundId", null)]
        [TestCase("payments/paymentId/refunds/refundId", false)]
        [TestCase("payments/paymentId/refunds/refundId?testmode=true", true)]
        public async Task GetRefundAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool? testModeParameter) {
            // Given: We make a request to retrieve a payment without wanting any extra data
            bool testMode = testModeParameter ?? false;
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}{expectedUrl}", defaultGetRefundResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("abcde", httpClient);

            // When: We send the request
            var refundResponse = await refundClient.GetRefundAsync("paymentId", "refundId", testmode: testMode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(refundResponse);
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CreateRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CreateRefundAsync(paymentId, new RefundRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetRefundListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundListAsync(paymentId: paymentId));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundAsync(paymentId, "refund-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetRefundAsync_NoRefundIsGiven_ArgumentExceptionIsThrown(string refundId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.GetRefundAsync("payment-id", refundId));

            // Then
            Assert.AreEqual($"Required URL argument 'refundId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CancelRefundAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelRefundAsync(paymentId, "refund-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CancelRefundAsync_NoRefundIsGiven_ArgumentExceptionIsThrown(string refundId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            RefundClient refundClient = new RefundClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await refundClient.CancelRefundAsync("payment-id", refundId));

            // Then
            Assert.AreEqual($"Required URL argument 'refundId' is null or empty", exception.Message); 
        }
    }
}
*/