using System;
using Mollie.Api.Client;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Models;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.List.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class CaptureClientTests : BaseClientTests {
        private const string defaultCaptureId = "tr_ffh435dvgs";
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultShipmentId = "shp_3wmsgCJN4U";
        private const string defaultSettlementId = "settlementId";
        private const string defaultAmountValue = "1027.99";
        private const string defaultAmountCurrency = "EUR";
        private const string defaultStatus = "succeeded";

        private string defaultCaptureJsonResponse = $@"{{
    ""resource"": ""capture"",
    ""id"": ""{defaultCaptureId}"",
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
    ""status"": ""{defaultStatus}"",
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
                ""status"": ""{defaultStatus}"",
                ""createdAt"": ""2018-08-02T09:29:56+00:00""
            }}
        ]
    }},
    ""count"": 1
}}";

        [Theory]
        [InlineData(true, "?testmode=true")]
        [InlineData(false, "")]
        public async Task GetCaptureAsync_CorrectQueryParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: We make a request to retrieve a capture
            const string paymentId = "payment-id";
            const string captureId = "capture-id";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}/captures/{captureId}{expectedQueryString}", defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            await captureClient.GetCaptureAsync(paymentId, captureId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Theory]
        [InlineData(true, "?testmode=true")]
        [InlineData(false, "")]
        public async Task GetCapturesListAsync_CorrectQueryParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: We make a request to retrieve a capture
            const string paymentId = "payment-id";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}/captures{expectedQueryString}", defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            await captureClient.GetCaptureListAsync(paymentId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task GetCaptureAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a capture with a payment id and capture id
            string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{defaultPaymentId}/captures/{defaultCaptureId}";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("api-key", httpClient);

            // When: We make the request
            CaptureResponse captureResponse = await captureClient.GetCaptureAsync(defaultPaymentId, defaultCaptureId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            captureResponse.ShouldNotBeNull();
            captureResponse.PaymentId.ShouldBe(defaultPaymentId);
            captureResponse.ShipmentId.ShouldBe(defaultShipmentId);
            captureResponse.SettlementId.ShouldBe(defaultSettlementId);
            captureResponse.Amount.Value.ShouldBe(defaultAmountValue);
            captureResponse.Amount.Currency.ShouldBe(defaultAmountCurrency);
            captureResponse.Status.ShouldBe(defaultStatus);
        }

        [Fact]
        public async Task GetCapturesListAsync_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{defaultPaymentId}/captures";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("api-key", httpClient);

            // When: We make the request
            ListResponse<CaptureResponse> listCaptureResponse = await captureClient.GetCaptureListAsync(defaultPaymentId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            listCaptureResponse.ShouldNotBeNull();
            listCaptureResponse.Count.ShouldBe(1);
            CaptureResponse captureResponse = listCaptureResponse.Items.First();
            captureResponse.PaymentId.ShouldBe(defaultPaymentId);
            captureResponse.ShipmentId.ShouldBe(defaultShipmentId);
            captureResponse.SettlementId.ShouldBe(defaultSettlementId);
            captureResponse.Amount.Value.ShouldBe(defaultAmountValue);
            captureResponse.Amount.Currency.ShouldBe(defaultAmountCurrency);
            captureResponse.Status.ShouldBe(defaultStatus);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetCaptureAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.GetCaptureAsync(paymentId, "capture-id"));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetCaptureAsync_NoCaptureIdIsGiven_ArgumentExceptionIsThrown(string? captureId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
#pragma warning disable CS8604 // Possible null reference argument.
                await captureClient.GetCaptureAsync("payment-id", captureId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'captureId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetCapturesListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.GetCaptureListAsync(paymentId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateCapture_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var captureRequest = new CaptureRequest {
                Amount = new Amount(Currency.EUR, 10m),
                Description = "capture-description"
            };
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await captureClient.CreateCapture(paymentId, captureRequest));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Fact]
        public async Task CreateCapture_CaptureIsCreated_CaptureIsDeserialized() {
            // Given
            var captureRequest = new CaptureRequest {
                Amount = new Amount(defaultAmountCurrency, defaultAmountValue),
                Description = "capture-description"
            };
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{defaultPaymentId}/captures",
                defaultCaptureJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CaptureClient captureClient = new CaptureClient("abcde", httpClient);

            // When
            CaptureResponse response = await captureClient.CreateCapture(defaultPaymentId, captureRequest);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
            response.Id.ShouldBe(defaultCaptureId);
            response.PaymentId.ShouldBe(defaultPaymentId);
        }
    }
}
