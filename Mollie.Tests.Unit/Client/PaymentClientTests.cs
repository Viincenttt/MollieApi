using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace Mollie.Tests.Unit.Client {
    public class PaymentClientTests : BaseClientTests {
        private const string defaultPaymentJsonResponse = @"{
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""redirectUrl"":""http://www.mollie.com""}";

        [Fact]
        public async Task CreatePaymentAsync_PaymentWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
            // Given: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com"
            };
            const string jsonToReturnInMockResponse = defaultPaymentJsonResponse;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
                .With(request => request.Headers.Contains("Idempotency-Key"))
                .Respond("application/json", jsonToReturnInMockResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

             // When: We send the request
             PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
        }

        [Fact]
        public async Task CreatePaymentAsync_PaymentWithSinglePaymentMethod_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with a single payment method
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Method = PaymentMethod.Ideal
            };
            string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\"";
            const string jsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"":""ideal"",
                ""redirectUrl"":""http://www.mollie.com""}";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
            paymentResponse.Method.Should().Be(paymentRequest.Method);
        }

        [Fact]
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
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedJsonResponse, expectedPaymentMethodJson);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
            paymentResponse.Method.Should().BeNull();
        }
        
        [Fact]
        public async Task CreatePayment_WithRoutingInformation_RequestIsSerializedInExpectedFormat() {
            // Given: We create a payment request with the routing request
            PaymentRoutingRequest routingRequest = new PaymentRoutingRequest {
                Amount = new Amount("EUR", 100),
                Destination = new RoutingDestination {
                    Type = "organization",
                    OrganizationId = "organization-id"
                },
                ReleaseDate = new DateTime(2022, 1, 14)
            };
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Routings = new List<PaymentRoutingRequest> {
                    routingRequest
                }
            };
            string expectedRoutingInformation = $"\"routing\":[{{\"amount\":{{\"currency\":\"EUR\",\"value\":\"100.00\"}},\"destination\":{{\"type\":\"organization\",\"organizationId\":\"organization-id\"}},\"releaseDate\":\"2022-01-14\"}}]}}";
            const string expectedJsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"": null,
                ""redirectUrl"":""http://www.mollie.com"",
                ""routing"": [{
                        ""amount"": {
                            ""currency"": ""EUR"",
                            ""value"": ""100.00""
                        },
                        ""destination"": {
                            ""type"": ""organization"",
                            ""organizationId"": ""organization-id""
                        },
                        ""releaseDate"": ""2022-01-14""
                    }
                ]}";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedJsonResponse, expectedRoutingInformation);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
            
            // When: We send the request
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
            paymentResponse.Method.Should().BeNull();
        }

        [Fact]
        public async Task CreatePaymentAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
            // Given: We make a request to create a payment and include the QR code
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Method = PaymentMethod.Ideal
            };
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.CreatePaymentAsync(paymentRequest, includeQrCode: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentAsync_NoIncludeParameters_QueryStringIsEmpty() {
            // Given: We make a request to retrieve a payment without wanting any extra data
            const string paymentId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentAsync(paymentId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetPaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.GetPaymentAsync(paymentId));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        [Fact]
        public async Task GetPaymentAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
            // Given: We make a request to retrieve a payment without wanting any extra data
            const string paymentId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?include=details.qrCode", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentAsync(paymentId, includeQrCode: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentAsync_IncludeRemainderDetails_QueryStringContainsIncludeRemainderDetailsParameter() {
            // Given: We make a request to retrieve a payment without wanting any extra data
            const string paymentId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?include=details.remainderDetails", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentAsync(paymentId, includeRemainderDetails: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentListAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
            // Given: We make a request to retrieve a payment without wanting any extra data
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentListAsync(includeQrCode: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentAsync_EmbedRefunds_QueryStringContainsEmbedRefundsParameter()
        {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string paymentId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?embed=refunds", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentAsync(paymentId, embedRefunds: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentListAsync_EmbedRefunds_QueryStringContainsEmbedRefundsParameter()
        {
            // Given: We make a request to retrieve a payment with embedded refunds
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?embed=refunds", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentListAsync(embedRefunds: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentAsync_EmbedChargebacks_QueryStringContainsEmbedChargebacksParameter()
        {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string paymentId = "abcde";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?embed=chargebacks", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentAsync(paymentId, embedChargebacks: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task GetPaymentListAsync_EmbedChargebacks_QueryStringContainsEmbedChargebacksParameter()
        {
            // Given: We make a request to retrieve a payment with embedded refunds
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?embed=chargebacks", defaultPaymentJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.GetPaymentListAsync(embedChargebacks: true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task DeletePaymentAsync_TestmodeIsTrue_RequestContainsTestmodeModel() {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string paymentId = "payment-id";
            string expectedContent = "\"testmode\":true";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Delete, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse, expectedContent);
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            await paymentClient.DeletePaymentAsync(paymentId, true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task DeletePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.DeletePaymentAsync(paymentId));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdatePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.UpdatePaymentAsync(paymentId, new PaymentUpdateRequest()));

            // Then
            exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
        }

        private void AssertPaymentIsEqual(PaymentRequest paymentRequest, PaymentResponse paymentResponse) {
            paymentResponse.Amount.Value.Should().Be(paymentRequest.Amount.Value);
            paymentResponse.Amount.Currency.Should().Be(paymentRequest.Amount.Currency);
            paymentResponse.Description.Should().Be(paymentRequest.Description);
            if (paymentRequest.Routings != null) {
                paymentResponse.Routings.Count.Should().Be(paymentRequest.Routings.Count);
                for (int i = 0; i < paymentRequest.Routings.Count; i++) {
                    var paymentRequestRouting = paymentRequest.Routings[i];
                    var paymentResponseRouting = paymentResponse.Routings[i];
                    paymentResponseRouting.Amount.Should().Be(paymentRequestRouting.Amount);
                    paymentResponseRouting.Destination.Type.Should().Be(paymentRequestRouting.Destination.Type);
                    paymentResponseRouting.Destination.OrganizationId.Should().Be(paymentRequestRouting.Destination.OrganizationId);
                    paymentResponseRouting.ReleaseDate.Should().Be(paymentRequestRouting.ReleaseDate);
                }
            }
        }
    }
}
