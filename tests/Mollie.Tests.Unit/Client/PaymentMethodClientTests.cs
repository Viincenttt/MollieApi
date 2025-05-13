using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
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

        [Fact]
        public async Task GetAllPaymentMethodListAsync_NoAmountParameter_QueryStringIsEmpty() {
            // Given: We make a request to retrieve all payment methods without any parameters
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}methods/all", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync();

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetAllPaymentMethodListAsync_AmountParameterIsAdded_QueryStringContainsAmount() {
            // Given: We make a request to retrieve all payment methods with a amount parameter
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}methods/all?amount[value]=100.00&amount[currency]=EUR", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync(amount: new Amount("EUR", 100));

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetAllPaymentMethodListAsync_ProfileIdParameterIsSpecified_QueryStringContainsProfileIdParameter() {
            // Given: We make a request to retrieve all payment methods with a profile id parameter
            var profileId = "myProfileId";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}methods/all?profileId={profileId}", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetAllPaymentMethodListAsync(profileId: profileId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentMethodListAsync_IncludeWalletsParameterIsSpecified_QueryStringContainsIncludeWalletsParameter() {
            // Given: We make a request to retrieve the payment methods with a includeWallets parameter
            var includeWalletsValue = "includeWalletsValue";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}methods?includeWallets={includeWalletsValue}", defaultPaymentMethodJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("abcde", httpClient);

            // When: We send the request
            await paymentMethodClient.GetPaymentMethodListAsync(includeWallets: includeWalletsValue);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetPaymentMethodAsync_NoPaymentMethodIsGiven_ArgumentExceptionIsThrown(string? paymentLinkId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentMethodClient.GetPaymentMethodAsync(paymentLinkId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentMethod' is null or empty");
        }
    }
}
