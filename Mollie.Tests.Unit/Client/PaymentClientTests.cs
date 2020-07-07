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
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
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
            const string jsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"":""ideal"",
                ""redirectUrl"":""http://www.mollie.com""}";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedPaymentMethodJson, jsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then            
            mockHttp.VerifyNoOutstandingExpectation();
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
            Assert.AreEqual(paymentRequest.Method, paymentResponse.Method);            
        }

        [Test]
        public async Task CreatePaymentAsync_PaymentWithMultiplePaymentMethods_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with multiple payment methods
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
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedPaymentMethodJson, expectedJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
            Assert.IsNull(paymentResponse.Method);
        }

        private void AssertPaymentIsEqual(PaymentRequest paymentRequest, PaymentResponse paymentResponse) {
            Assert.AreEqual(paymentRequest.Amount.Value, paymentResponse.Amount.Value);
            Assert.AreEqual(paymentRequest.Amount.Currency, paymentResponse.Amount.Currency);
            Assert.AreEqual(paymentRequest.Description, paymentResponse.Description);
        }

        private MockHttpMessageHandler CreateMockHttpMessageHandler(HttpMethod httpMethod, string url, string expectedPartialContent, string response) {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(httpMethod, url)
                .WithPartialContent(expectedPartialContent)
                .Respond("application/json", response);

            return mockHttp;
        }
    }
}
