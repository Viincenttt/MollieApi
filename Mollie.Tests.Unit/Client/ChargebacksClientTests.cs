using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Chargeback;
using NUnit.Framework;
using RichardSzalay.MockHttp;

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
        public async Task CreatePaymentAsync_PaymentWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
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
    }
}