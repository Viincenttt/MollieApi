using Mollie.Api.Client;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

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
    }
}
