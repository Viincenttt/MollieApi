using System;
using Mollie.Api.Client;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.List;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Capture.Request;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client {
    public class CaptureClientTests : BaseClientTests {
        private const string defaultCaptureId = "tr_ffh435dvgs";
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultShipmentId = "shp_3wmsgCJN4U";
        private const string defaultSettlementId = "settlementId";
        private const string defaultAmountValue = "1027.99";
        private const string defaultAmountCurrency = "EUR";

        private string defaultCaptureJsonResponse = $@"{{
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
    ""createdAt"": ""2018-08-02T09:29:56+00:00"",
}}";

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

        [TestCase(true, "?testmode=true")]
        [TestCase(false, "")]
        public async Task GetCaptureAsync_CorrectQueryParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: We make a request to retrieve a capture
            const string paymentId = "payment-id";
            const string captureId = "capture-id";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}/captures/{captureId}{expectedQueryString}", defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            await captureClient.GetCaptureAsync(paymentId, captureId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }
        
        [TestCase(true, "?testmode=true")]
        [TestCase(false, "")]
        public async Task GetCapturesListAsync_CorrectQueryParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: We make a request to retrieve a capture
            const string paymentId = "payment-id";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}/captures{expectedQueryString}", defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            await captureClient.GetCapturesListAsync(paymentId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }
        
        [Test]
        public async Task GetCaptureAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a capture with a payment id and capture id
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}payments/{defaultPaymentId}/captures/{defaultCaptureId}";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("api-key", httpClient);

            // When: We make the request
            CaptureResponse captureResponse = await captureClient.GetCaptureAsync(defaultPaymentId, defaultCaptureId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(captureResponse);
            Assert.AreEqual(defaultPaymentId, captureResponse.PaymentId);
            Assert.AreEqual(defaultShipmentId, captureResponse.ShipmentId);
            Assert.AreEqual(defaultSettlementId, captureResponse.SettlementId);
            Assert.AreEqual(defaultAmountValue, captureResponse.Amount.Value);
            Assert.AreEqual(defaultAmountCurrency, captureResponse.Amount.Currency);
        }

        [Test]
        public async Task GetCapturesListAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}payments/{defaultPaymentId}/captures";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("api-key", httpClient);

            // When: We make the request
            ListResponse<CaptureResponse> listCaptureResponse = await captureClient.GetCapturesListAsync(defaultPaymentId);

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
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetCaptureAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.GetCaptureAsync(paymentId, "capture-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetCaptureAsync_NoCaptureIdIsGiven_ArgumentExceptionIsThrown(string captureId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.GetCaptureAsync("payment-id", captureId));

            // Then
            Assert.AreEqual($"Required URL argument 'captureId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetCapturesListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.GetCapturesListAsync(paymentId));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CreateCapture_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var captureRequest = new CaptureRequest {
                Amount = new Amount(Currency.EUR, 10m),
                Description = "capture-description"
            };
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.CreateCapture(paymentId, captureRequest));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
    }
}
