using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Chargeback;
using RichardSzalay.MockHttp;
/*
namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class ChargebacksClientTests : BaseClientTests {
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
        
        [Test]
        public async Task GetChargebackAsync_ResponseIsDeserializedInExpectedFormat() {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("abcde", httpClient); 

            // When: We send the request
            ChargebackResponse chargebackResponse = await chargebacksClient.GetChargebackAsync(defaultPaymentId, defaultChargebackId);

            // Then
            Assert.AreEqual(defaultPaymentId, chargebackResponse.PaymentId);
            Assert.AreEqual(defaultChargebackId, chargebackResponse.Id);
            Assert.IsNotNull(chargebackResponse.Reason);
            Assert.AreEqual(defaultChargebackReasonCode, chargebackResponse.Reason.Code);
            Assert.AreEqual(defaultChargebackReason, chargebackResponse.Reason.Description);
        }
        
        [Test]
        [TestCase(false, "")]
        [TestCase(false, "?testmode=true")]
        public async Task GetOrderRefundListAsync_QueryParameterOptions_CorrectParametersAreAdded(bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}payments/{defaultPaymentId}/chargebacks/{defaultChargebackId}")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("abcde", httpClient); 

            // When: We send the request
            await chargebacksClient.GetChargebackAsync(defaultPaymentId, defaultChargebackId);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }
        
        [TestCase(null, null, false, "")]
        [TestCase("from", null, false, "?from=from")]
        [TestCase("from", 50, false, "?from=from&limit=50")]
        [TestCase(null, null, true, "?testmode=true")]
        public async Task GetChargebacksListAsync_QueryParameterOptions_CorrectParametersAreAdded(string from, int? limit, bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}payments/{defaultPaymentId}/chargebacks")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("abcde", httpClient); 

            // When: We send the request
            await chargebacksClient.GetChargebacksListAsync(defaultPaymentId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }
        
        [Test]
        [TestCase(null, false, "")]
        [TestCase("profileId", false, "?profileId=profileId")]
        [TestCase("profileId", true, "?profileId=profileId&testmode=true")]
        public async Task GetChargebacksListAsync_QueryParameterOptions_CorrectParametersAreAdded(string profileId, bool testmode, string expectedQueryString) {
            // Given: we retrieve the chargeback by id and payment id
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}chargebacks")
                .Respond("application/json", defaultGetChargebacksResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("abcde", httpClient); 

            // When: We send the request
            await chargebacksClient.GetChargebacksListAsync(profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetChargebackAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await chargebacksClient.GetChargebackAsync(paymentId, "chargeback-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetChargebackAsync_NoChargeBackIdIsGiven_ArgumentExceptionIsThrown(string chargebackId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await chargebacksClient.GetChargebackAsync("payment-id", chargebackId));

            // Then
            Assert.AreEqual($"Required URL argument 'chargebackId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetChargebacksListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            ChargebacksClient chargebacksClient = new ChargebacksClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await chargebacksClient.GetChargebacksListAsync(paymentId: paymentId));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentId' is null or empty", exception.Message); 
        }
    }
}*/