using Mollie.Api.Client;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.List;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class SettlementClientTests : BaseClientTests {
        private const string defaultSettlementId = "tr_Agfg241g";
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultShipmentId = "shp_3wmsgCJN4U";
        private const string defaultAmountValue = "1027.99";
        private const string defaultAmountCurrency = "EUR";

        private string defaultCaptureListJsonResponse = $@"{{
    ""_embedded"": {{
        ""captures"": [
            {{
                ""resource"": ""capture"",
                ""id"": ""cpt_4qqhO89gsT"",
                ""mode"": ""live"",
                ""amount"": {{
                    ""value"": ""{defaultAmountValue}"",
                    ""currency"": ""{defaultAmountCurrency}""
                }},
                ""settlementAmount"": {{
                    ""value"": ""1027.99"",
                    ""currency"": ""EUR""
                }},
                ""paymentId"": ""{defaultPaymentId}"",
                ""shipmentId"": ""{defaultShipmentId}"",
                ""settlementId"": ""{defaultSettlementId}"",
                ""createdAt"": ""2018-08-02T09:29:56+00:00""
            }}
        ]
    }},
    ""count"": 1
}}";

        [Test]
        public async Task ListSettlementCaptures_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/{defaultSettlementId}/captures";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementsClient settlementsClient = new SettlementsClient("api-key", httpClient);

            // When: We make the request
            ListResponse<CaptureResponse> listCaptureResponse = await settlementsClient.ListSettlementCapturesAsync(defaultSettlementId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(listCaptureResponse);
            Assert.AreEqual(1, listCaptureResponse.Count);
            CaptureResponse captureResponse = listCaptureResponse.Items.First();
            Assert.AreEqual(defaultPaymentId, captureResponse.PaymentId);
            Assert.AreEqual(defaultShipmentId, captureResponse.ShipmentId);
            Assert.AreEqual(defaultSettlementId, captureResponse.SettlementId);
            Assert.AreEqual(defaultAmountValue, captureResponse.Amount.Value);
            Assert.AreEqual(defaultAmountCurrency, captureResponse.Amount.Currency);
        }
    }
}
