using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class PaymentClientTests {
        [Test]
        public async Task CreatePaymentAsync_PaymentWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
            // Given: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com"
            };
            const string jsonToReturnInMockResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""redirectUrl"":""http://www.mollie.com""}";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .Respond("application/json", jsonToReturnInMockResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient); 

             // When: We send the request
             PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            Assert.AreEqual(paymentRequest.Amount.Value, paymentResponse.Amount.Value);
            Assert.AreEqual(paymentRequest.Amount.Currency, paymentResponse.Amount.Currency);
            Assert.AreEqual(paymentRequest.Description, paymentResponse.Description);
        }
        
        [Test]
        public async Task CreatePaymentAsync_PaymentWithSinglePaymentMethod_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with a single payment method
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Method = PaymentMethod.Ideal
            };
            const string expectedPaymentMethodJson = "\"method\":[\"ideal\"";
            const string expectedJsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"":""ideal"",
                ""redirectUrl"":""http://www.mollie.com""}";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments")
                .WithPartialContent(expectedPaymentMethodJson)
                .Respond("application/json", expectedJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then            
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.AreEqual(paymentRequest.Amount.Value, paymentResponse.Amount.Value);
            Assert.AreEqual(paymentRequest.Amount.Currency, paymentResponse.Amount.Currency);
            Assert.AreEqual(paymentRequest.Description, paymentResponse.Description);
            Assert.AreEqual(paymentRequest.Method, paymentResponse.Method);            
        }

        [Test]
        public async Task CreatePaymentAsync_PaymentWithMultiplePaymentMethods_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with a single payment method
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Methods = new List<string>() {
                    PaymentMethod.Ideal,
                    PaymentMethod.CreditCard,
                    PaymentMethod.DirectDebit
                }
            };
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\",\"{PaymentMethod.CreditCard}\",\"{PaymentMethod.DirectDebit}\"]";
            const string expectedJsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"": null,
                ""redirectUrl"":""http://www.mollie.com""}";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments")
                .WithPartialContent(expectedPaymentMethodJson)
                .Respond("application/json", expectedJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then            
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.AreEqual(paymentRequest.Amount.Value, paymentResponse.Amount.Value);
            Assert.AreEqual(paymentRequest.Amount.Currency, paymentResponse.Amount.Currency);
            Assert.AreEqual(paymentRequest.Description, paymentResponse.Description);
            Assert.IsNull(paymentResponse.Method);
        }
    }
}
