using Mollie.Api.Client;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class OrderClientTests : BaseClientTests {
        private const string defaultOrderJsonResponse = @"{
            ""resource"": ""order"",
            ""id"": ""ord_kEn1PlbGa"",
            ""profileId"": ""pfl_URR55HPMGx"",
            ""method"": ""ideal"",
            ""amount"": {
                ""value"": ""1027.99"",
                ""currency"": ""EUR""
            },
        }";

        [Test]
        public async Task GetOrderAsync_NoEmbedParameters_QueryStringIsEmpty() {
            // Given: We make a request to retrieve a order without wanting any extra data
            const string orderId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task GetOrderAsync_SingleEmbedParameters_QueryStringContainsEmbedParameter() {
            // Given: We make a request to retrieve a order with a single embed parameter
            const string orderId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}?embed=payments", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId, embedPayments: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task GetOrderAsync_MultipleEmbedParameters_QueryStringContainsMultipleParameters() {
            // Given: We make a request to retrieve a order with a single embed parameter
            const string orderId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}?embed=payments,refunds,shipments", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId, embedPayments: true, embedRefunds: true, embedShipments: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
