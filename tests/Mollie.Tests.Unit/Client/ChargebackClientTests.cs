using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models.Chargeback.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class ChargebackClientTests : BaseClientTests {
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultChargebackId = "chb_n9z0tp";
        private const string defaultChargebackReasonCode = "AC01";
        private const string defaultChargebackReason = "Account identifier incorrect (i.e. invalid IBAN)";

        private string defaultGetChargebacksResponse = @$"{{
    ""resource"": ""chargeback"",
    ""id"": ""{defaultChargebackId}"",
    ""amount"": {{
        ""currency"": ""USD"",
        ""value"": ""43.38""
    }},
    ""settlementAmount"": {{
        ""currency"": ""EUR"",
        ""value"": ""-35.07""
    }},
    ""createdAt"": ""2018-03-14T17:00:52.0Z"",
     ""reason"": {{
       ""code"": ""AC01"",
       ""description"": ""Account identifier incorrect (i.e. invalid IBAN)""
     }},
    ""reversedAt"": null,
    ""paymentId"": ""{defaultPaymentId}"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg/chargebacks/chb_n9z0tp"",
            ""type"": ""application/hal+json""
        }},
        ""payment"": {{
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/chargebacks-api/get-chargeback"",
            ""type"": ""text/html""
        }}
    }}
}}";

        [Fact]
        public async Task GetChargebackAsync_ResponseIsDeserializedInExpectedFormat() {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}*")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("abcde", httpClient);

            // When: We send the request
            ChargebackResponse chargebackResponse = await chargebackClient.GetChargebackAsync(defaultPaymentId, defaultChargebackId);

            // Then
            chargebackResponse.PaymentId.ShouldBe(defaultPaymentId);
            chargebackResponse.Id.ShouldBe(defaultChargebackId);
            chargebackResponse.Reason.ShouldNotBeNull();
            chargebackResponse.Reason!.Code.ShouldBe(defaultChargebackReasonCode);
            chargebackResponse.Reason.Description.ShouldBe(defaultChargebackReason);
        }

        [Theory]
        [InlineData(false, "")]
        [InlineData(true, "?testmode=true")]
        public async Task GetOrderRefundListAsync_QueryParameterOptions_CorrectParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{defaultPaymentId}/chargebacks/{defaultChargebackId}{expectedQueryString}")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("abcde", httpClient);

            // When: We send the request
            await chargebackClient.GetChargebackAsync(defaultPaymentId, defaultChargebackId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Theory]
        [InlineData(null, null, false, "")]
        [InlineData("from", null, false, "?from=from")]
        [InlineData("from", 50, false, "?from=from&limit=50")]
        [InlineData(null, null, true, "?testmode=true")]
        public async Task GetChargebacksListAsync_FromLimitTestmodeQueryParameterOptions_CorrectParametersAreAdded(string? from, int? limit, bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{defaultPaymentId}/chargebacks{expectedQueryString}")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("abcde", httpClient);

            // When: We send the request
            await chargebackClient.GetChargebackListAsync(defaultPaymentId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Theory]
        [InlineData(null, false, "")]
        [InlineData("profileId", false, "?profileId=profileId")]
        [InlineData("profileId", true, "?profileId=profileId&testmode=true")]
        public async Task GetChargebacksListAsync_ProfileTestModeQueryParameterOptions_CorrectParametersAreAdded(string? profileId, bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}chargebacks{expectedQueryString}")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("abcde", httpClient);

            // When: We send the request
            await chargebackClient.GetChargebackListAsync(profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetChargebackAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await chargebackClient.GetChargebackAsync(paymentId, "chargeback-id"));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetChargebackAsync_NoChargeBackIdIsGiven_ArgumentExceptionIsThrown(string? chargebackId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await chargebackClient.GetChargebackAsync("payment-id", chargebackId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'chargebackId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetChargebacksListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebackClient chargebackClient = new ChargebackClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await chargebackClient.GetChargebackListAsync(paymentId: paymentId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }
    }
}
