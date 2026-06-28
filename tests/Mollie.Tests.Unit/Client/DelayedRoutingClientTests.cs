using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.DelayedRouting.Request;
using Mollie.Api.Models.DelayedRouting.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class DelayedRoutingClientTests : BaseClientTests {
        private const string DefaultRouteId = "crt_dyARQ3JzCgtPDhU2Pbq3J";
        private const string DefaultPaymentId = "tr_5B8cwPMGnU6qLbRvo7qEZo";
        private const string DefaultOrganizationId = "org_12345678";
        private const string DefaultAmountValue = "10.00";
        private const string DefaultAmountCurrency = "EUR";

        private readonly string _defaultRouteJsonResponse = $@"{{
    ""resource"": ""route"",
    ""id"": ""{DefaultRouteId}"",
    ""paymentId"": ""{DefaultPaymentId}"",
    ""amount"": {{
        ""value"": ""{DefaultAmountValue}"",
        ""currency"": ""{DefaultAmountCurrency}""
    }},
    ""description"": ""Route for customer payout"",
    ""destination"": {{
        ""type"": ""organization"",
        ""organizationId"": ""{DefaultOrganizationId}""
    }},
    ""createdAt"": ""2024-01-15T10:00:00+00:00"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/payments/{DefaultPaymentId}/routes/{DefaultRouteId}"",
            ""type"": ""application/hal+json""
        }},
        ""payment"": {{
            ""href"": ""https://api.mollie.com/v2/payments/{DefaultPaymentId}"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/create-route"",
            ""type"": ""text/html""
        }}
    }}
}}";

        private readonly string _defaultRouteListJsonResponse = $@"{{
    ""count"": 1,
    ""_embedded"": {{
        ""routes"": [
            {{
                ""resource"": ""route"",
                ""id"": ""{DefaultRouteId}"",
                ""paymentId"": ""{DefaultPaymentId}"",
                ""amount"": {{
                    ""value"": ""{DefaultAmountValue}"",
                    ""currency"": ""{DefaultAmountCurrency}""
                }},
                ""description"": ""Route for customer payout"",
                ""destination"": {{
                    ""type"": ""organization"",
                    ""organizationId"": ""{DefaultOrganizationId}""
                }},
                ""createdAt"": ""2024-01-15T10:00:00+00:00"",
                ""_links"": {{
                    ""self"": {{
                        ""href"": ""https://api.mollie.com/v2/payments/{DefaultPaymentId}/routes/{DefaultRouteId}"",
                        ""type"": ""application/hal+json""
                    }},
                    ""payment"": {{
                        ""href"": ""https://api.mollie.com/v2/payments/{DefaultPaymentId}"",
                        ""type"": ""application/hal+json""
                    }},
                    ""documentation"": {{
                        ""href"": ""https://docs.mollie.com/reference/create-route"",
                        ""type"": ""text/html""
                    }}
                }}
            }}
        ]
    }},
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/payments/{DefaultPaymentId}/routes"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/list-payment-routes"",
            ""type"": ""text/html""
        }}
    }}
}}";

        [Fact]
        public async Task CreateDelayedRouteAsync_DefaultBehaviour_ResponseIsDeserializedCorrectly() {
            // Given
            var request = new DelayedRoutingRequest {
                Amount = new Amount(DefaultAmountCurrency, DefaultAmountValue),
                Destination = new RoutingDestination {
                    Type = "organization",
                    OrganizationId = DefaultOrganizationId
                },
                Description = "Route for customer payout"
            };
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes",
                _defaultRouteJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            DelayedRoutingResponse result = await client.CreateDelayedRouteAsync(DefaultPaymentId, request);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
            result.Id.ShouldBe(DefaultRouteId);
            result.Resource.ShouldBe("route");
            result.PaymentId.ShouldBe(DefaultPaymentId);
            result.Amount.Value.ShouldBe(DefaultAmountValue);
            result.Amount.Currency.ShouldBe(DefaultAmountCurrency);
            result.Description.ShouldBe("Route for customer payout");
            result.Destination.ShouldNotBeNull();
            result.Destination.Type.ShouldBe("organization");
            result.Destination.OrganizationId.ShouldBe(DefaultOrganizationId);
            result.CreatedAt.ShouldBeOfType<DateTime>();
            result.Links.ShouldNotBeNull();
            result.Links.Self.ShouldNotBeNull();
            result.Links.Payment.ShouldNotBeNull();
            result.Links.Documentation.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateDelayedRouteAsync_TestmodeIsSet_TestmodeIsSerializedInRequestBody(bool testmode) {
            // Given
            var request = new DelayedRoutingRequest {
                Amount = new Amount(DefaultAmountCurrency, DefaultAmountValue),
                Destination = new RoutingDestination {
                    Type = "organization",
                    OrganizationId = DefaultOrganizationId
                },
                Testmode = testmode
            };
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes",
                _defaultRouteJsonResponse,
                $"\"testmode\":{testmode.ToString().ToLower()}");
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            await client.CreateDelayedRouteAsync(DefaultPaymentId, request);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateDelayedRouteAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var request = new DelayedRoutingRequest {
                Amount = new Amount(Currency.EUR, 10m),
                Destination = new RoutingDestination {
                    Type = "organization",
                    OrganizationId = DefaultOrganizationId
                }
            };
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
#pragma warning disable CS8604
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await client.CreateDelayedRouteAsync(paymentId, request));
#pragma warning restore CS8604

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Fact]
        public async Task GetPaymentRouteListAsync_DefaultBehaviour_ResponseIsDeserializedCorrectly() {
            // Given
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes",
                _defaultRouteListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            ListResponse<DelayedRoutingResponse> result = await client.GetPaymentRouteListAsync(DefaultPaymentId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            result.Items.ShouldHaveSingleItem();
            var route = result.Items[0];
            route.Id.ShouldBe(DefaultRouteId);
            route.PaymentId.ShouldBe(DefaultPaymentId);
            route.Amount.Value.ShouldBe(DefaultAmountValue);
            route.Amount.Currency.ShouldBe(DefaultAmountCurrency);
            route.Destination.Type.ShouldBe("organization");
            route.Destination.OrganizationId.ShouldBe(DefaultOrganizationId);
        }

        [Theory]
        [InlineData(true, "?testmode=true")]
        [InlineData(false, "")]
        public async Task GetPaymentRouteListAsync_TestmodeQueryParameter_IsAddedCorrectly(bool testmode, string expectedQueryString) {
            // Given
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes{expectedQueryString}",
                _defaultRouteListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            await client.GetPaymentRouteListAsync(DefaultPaymentId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetPaymentRouteListAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
#pragma warning disable CS8604
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await client.GetPaymentRouteListAsync(paymentId));
#pragma warning restore CS8604

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Fact]
        public async Task GetDelayedRouteAsync_DefaultBehaviour_ResponseIsDeserializedCorrectly() {
            // Given
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes/{DefaultRouteId}",
                _defaultRouteJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            DelayedRoutingResponse result = await client.GetDelayedRouteAsync(DefaultPaymentId, DefaultRouteId);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
            result.Id.ShouldBe(DefaultRouteId);
            result.Resource.ShouldBe("route");
            result.PaymentId.ShouldBe(DefaultPaymentId);
            result.Amount.Value.ShouldBe(DefaultAmountValue);
            result.Amount.Currency.ShouldBe(DefaultAmountCurrency);
            result.Destination.Type.ShouldBe("organization");
            result.Destination.OrganizationId.ShouldBe(DefaultOrganizationId);
        }

        [Theory]
        [InlineData(true, "?testmode=true")]
        [InlineData(false, "")]
        public async Task GetDelayedRouteAsync_TestmodeQueryParameter_IsAddedCorrectly(bool testmode, string expectedQueryString) {
            // Given
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{DefaultPaymentId}/routes/{DefaultRouteId}{expectedQueryString}",
                _defaultRouteJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
            await client.GetDelayedRouteAsync(DefaultPaymentId, DefaultRouteId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetDelayedRouteAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
#pragma warning disable CS8604
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await client.GetDelayedRouteAsync(paymentId, DefaultRouteId));
#pragma warning restore CS8604

            // Then
            exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetDelayedRouteAsync_NoRouteIdIsGiven_ArgumentExceptionIsThrown(string? routeId) {
            // Given
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var client = new DelayedRoutingClient("test_api_key", httpClient);

            // When
#pragma warning disable CS8604
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await client.GetDelayedRouteAsync(DefaultPaymentId, routeId));
#pragma warning restore CS8604

            // Then
            exception.Message.ShouldBe("Required URL argument 'routeId' is null or empty");
        }
    }
}




