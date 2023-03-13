using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class PaymentMethodClientTests : BaseClientTests {
        private const string defaultPaymentMethodJsonResponse = @"{
            ""count"": 13,
            ""_embedded"": {
                ""methods"": [
                    {
                         ""resource"": ""method"",
                         ""id"": ""ideal"",
                         ""description"": ""iDEAL""
                    }
                ]
            }
        }";

        [Test]
        public async Task GetAllPaymentMethodListAsync_NoAmountParameter_QueryStringIsEmpty() {
            // Given: We make a request to retrieve all payment methods without any parameters
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}methods/all", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync();

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task GetAllPaymentMethodListAsync_AmountParameterIsAdded_QueryStringContainsAmount() {
            // Given: We make a request to retrieve all payment methods with a amount parameter
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}methods/all?amount[value]=100.00&amount[currency]=EUR", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync(amount: new Amount("EUR", 100));

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task GetAllPaymentMethodListAsync_ProfileIdParameterIsSpecified_QueryStringContainsProfileIdParameter() {
            // Given: We make a request to retrieve all payment methods with a profile id parameter
            var profileId = "myProfileId";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}methods/all?profileId={profileId}", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync(profileId: profileId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Test]
        public async Task GetPaymentMethodListAsync_IncludeWalletsParameterIsSpecified_QueryStringContainsIncludeWalletsParameter() {
            // Given: We make a request to retrieve the payment methods with a includeWallets parameter
            var includeWalletsValue = "includeWalletsValue";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}methods?includeWallets={includeWalletsValue}", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetPaymentMethodListAsync(includeWallets: includeWalletsValue);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetPaymentMethodAsync_NoPaymentLinkIdIsGiven_ArgumentExceptionIsThrown(string paymentLinkId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await paymentMethodClient.GetPaymentMethodAsync(paymentLinkId));

            // Then
            Assert.AreEqual($"Required URL argument 'paymentMethod' is null or empty", exception.Message); 
        }
    }
}
