using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Payment;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Order.Response;
using RichardSzalay.MockHttp;
using Xunit;

using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Unit.Client {
    public class OrderClientTests : BaseClientTests {
        [Fact]
        public async Task GetOrderAsync_NoEmbedParameters_QueryStringIsEmpty() {
            // Given: We make a request to retrieve a order without wanting any extra data
            const string orderId = "abcde";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetOrderAsync_SingleEmbedParameters_QueryStringContainsEmbedParameter() {
            // Given: We make a request to retrieve a order with a single embed parameter
            const string orderId = "abcde";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}?embed=payments", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId, embedPayments: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetOrderAsync_MultipleEmbedParameters_QueryStringContainsMultipleParameters() {
            // Given: We make a request to retrieve a order with a single embed parameter
            const string orderId = "abcde";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}?embed=payments,refunds,shipments", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId, embedPayments: true, embedRefunds: true, embedShipments: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetOrderAsync_WithTestModeParameter_QueryStringContainsTestModeParameter() {
            // Given: We make a request to retrieve a order with a single embed parameter
            const string orderId = "abcde";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders/{orderId}?testmode=true", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderAsync(orderId, testmode: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null, null, null, false, null, "")]
        [InlineData("from", null, null, false, null, "?from=from")]
        [InlineData("from", 50, null, false, null, "?from=from&limit=50")]
        [InlineData(null, null, "profile-id", false, null, "?profileId=profile-id")]
        [InlineData(null, null, "profile-id", true, null, "?profileId=profile-id&testmode=true")]
        [InlineData(null, null, "profile-id", true, SortDirection.Desc, "?profileId=profile-id&testmode=true&sort=desc")]
        [InlineData(null, null, "profile-id", true, SortDirection.Asc, "?profileId=profile-id&testmode=true&sort=asc")]
        public async Task GetOrderListAsync_QueryParameterOptions_CorrectParametersAreAdded(
            string? from,
            int? limit,
            string? profileId,
            bool testmode,
            SortDirection? sortDirection,
            string expectedQueryString) {
            // Given: We make a request to retrieve the list of orders
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}orders{expectedQueryString}", defaultOrderJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.GetOrderListAsync(from, limit, profileId, testmode, sortDirection);

            // Then
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task CreateOrderAsync_SinglePaymentMethod_RequestIsSerializedInExpectedFormat() {
            // Given: we create a order with a single payment method
            OrderRequest orderRequest = CreateOrderRequestWithOnlyRequiredFields();
            orderRequest.Method = PaymentMethod.Ideal;
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}";
            const string jsonResponse = defaultOrderJsonResponse;
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}orders", jsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            OrderResponse orderResponse = await orderClient.CreateOrderAsync(orderRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            orderResponse.Method.ShouldBe(orderRequest.Method);
        }

        [Fact]
        public async Task CreateOrderAsync_MultiplePaymentMethods_RequestIsSerializedInExpectedFormat() {
            // Given: we create a order with a single payment method
            OrderRequest orderRequest = CreateOrderRequestWithOnlyRequiredFields();
            orderRequest.Methods = new List<string>() {
                PaymentMethod.Ideal,
                PaymentMethod.CreditCard,
                PaymentMethod.DirectDebit
            };
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\",\"{PaymentMethod.CreditCard}\",\"{PaymentMethod.DirectDebit}\"]";
            const string jsonResponse = defaultOrderJsonResponse;
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}orders", jsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.CreateOrderAsync(orderRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CreateOrderPaymentAsync_PaymentWithSinglePaymentMethod_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with multiple payment methods
            OrderPaymentRequest orderPaymentRequest = new OrderPaymentRequest() {
                CustomerId = "customer-id",
                Testmode = true,
                MandateId = "mandate-id",
                Methods = new List<string>() {
                    PaymentMethod.Ideal
                }
            };
            const string orderId = "order-id";
            string url = $"{BaseMollieClient.ApiEndPoint}orders/{orderId}/payments";
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\"]";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, url, defaultPaymentJsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.CreateOrderPaymentAsync(orderId, orderPaymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CreateOrderPaymentAsync_PaymentWithMultiplePaymentMethods_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with multiple payment methods
            OrderPaymentRequest orderPaymentRequest = new OrderPaymentRequest() {
                CustomerId = "customer-id",
                Testmode = true,
                MandateId = "mandate-id",
                Methods = new List<string>() {
                    PaymentMethod.Ideal,
                    PaymentMethod.CreditCard,
                    PaymentMethod.DirectDebit
                }
            };
            const string orderId = "order-id";
            string url = $"{BaseMollieClient.ApiEndPoint}orders/{orderId}/payments";
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\",\"{PaymentMethod.CreditCard}\",\"{PaymentMethod.DirectDebit}\"]";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, url, defaultPaymentJsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("abcde", httpClient);

            // When: We send the request
            await orderClient.CreateOrderPaymentAsync(orderId, orderPaymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetOrderAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.GetOrderAsync(orderId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateOrderAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.UpdateOrderAsync(orderId, new OrderUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateOrderLinesAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.UpdateOrderLinesAsync(orderId, "order-line-id", new OrderLineUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateOrderLinesAsync_NoOrderLineIdIsGiven_ArgumentExceptionIsThrown(string? orderLineId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.UpdateOrderLinesAsync("order-id", orderLineId, new OrderLineUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderLineId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task ManageOrderLinesAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);
            ManageOrderLinesRequest request = new ManageOrderLinesRequest {
                Operations = new List<ManageOrderLinesOperation> {
                    new ManageOrderLinesUpdateOperation {
                        Data = new ManageOrderLinesUpdateOperationData
                        {
                            Id = "id"
                        }
                    }
                }
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
#pragma warning disable CS8604 // Possible null reference argument.
                await orderClient.ManageOrderLinesAsync(orderId, request));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelOrderAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.CancelOrderAsync(orderId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateOrderPaymentAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string? orderId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            OrderClient orderClient = new OrderClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await orderClient.CreateOrderPaymentAsync(orderId, new OrderPaymentRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'orderId' is null or empty");
        }

        private OrderRequest CreateOrderRequestWithOnlyRequiredFields() {
            return new OrderRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                OrderNumber = "16738",
                Lines = new List<OrderLineRequest>() {
                    new OrderLineRequest() {
                        Name = "A box of chocolates",
                        Quantity = 1,
                        UnitPrice = new Amount(Currency.EUR, "100.00"),
                        TotalAmount = new Amount(Currency.EUR, "100.00"),
                        VatRate = "21.00",
                        VatAmount = new Amount(Currency.EUR, "17.36")
                    }
                },
                BillingAddress = new OrderAddressDetails() {
                    GivenName = "John",
                    FamilyName = "Smit",
                    Email = "johnsmit@gmail.com",
                    City = "Rotterdam",
                    Country = "NL",
                    PostalCode = "0000AA",
                    Region = "Zuid-Holland",
                    StreetAndNumber = "Coolsingel 1"
                },
                RedirectUrl = "http://www.google.nl",
                Locale = Locale.nl_NL
            };
        }

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

        private const string defaultPaymentJsonResponse = @"{
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""redirectUrl"":""http://www.mollie.com""}";

    }
}
