using Mollie.Api.Client;
using Mollie.Api.Models;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

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
            // Given: We make a request to retrieve a order without wanting any extra data
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
            // Given: We make a request to retrieve a order without wanting any extra data
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
            // Given: We make a request to retrieve a order without wanting any extra data
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
            // Given: We make a request to retrieve a order without wanting any extra data
            var includeWalletsValue = "includeWalletsValue";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}methods?includeWallets={includeWalletsValue}", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetPaymentMethodListAsync(includeWallets: includeWalletsValue);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
