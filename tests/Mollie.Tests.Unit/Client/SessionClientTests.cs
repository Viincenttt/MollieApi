using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Session;
using Mollie.Api.Models.Session.Request;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class SessionClientTests : BaseClientTests {
        private const string DefaultSessionId = "sess_pbjz8x";

        private const string DefaultSessionJsonResponse = @"{
    ""resource"": ""session"",
    ""id"": ""sess_pbjz8x"",
    ""mode"": ""test"",
    ""clientAccessToken"": ""access_token"",
    ""status"": ""completed"",
    ""amount"": {
        ""value"": ""10.00"",
        ""currency"": ""EUR""
    },
    ""description"": ""Description"",
    ""redirectUrl"": ""https://www.mollie.com"",
    ""profileId"": ""pfl_QkEhN94Ba"",
    ""customerId"": null,
    ""sequenceType"": ""oneoff"",
    ""requiredCustomerDetails"": [""billing-address"", ""shipping-address""],
    ""createdAt"": ""2026-10-01T10:13:00+09:00"",
    ""expiredAt"": ""2026-10-01T10:00:00+00:00"",
    ""completedAt"": ""2026-10-01T09:30:00+00:00"",
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/sessions/sess_pbjz8x"",
            ""type"": ""application/hal+json""
        }
    }
}";

        [Fact]
        public async Task GetSessionAsync_ResponseIsDeserializedInExpectedFormat() {
            // Given: We retrieve a session
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Get,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}sessions/{DefaultSessionId}",
                DefaultSessionJsonResponse);
            using var sessionClient = new SessionClient("abcde", mockHttp.ToHttpClient());

            // When: We make the request
            var result = await sessionClient.GetSessionAsync(DefaultSessionId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            result.Success.ShouldBeTrue();
            result.Data.Id.ShouldBe(DefaultSessionId);
            result.Data.CustomerId.ShouldBeNull();
            result.Data.RequiredCustomerDetails.ShouldBe([SessionRequiredCustomerDetail.BillingAddress, SessionRequiredCustomerDetail.ShippingAddress]);
            result.Data.CreatedAt.ShouldBe(new DateTimeOffset(2026, 10, 1, 10, 13, 0, TimeSpan.FromHours(9)));
            result.Data.CreatedAt.Offset.ShouldBe(TimeSpan.FromHours(9));
            result.Data.ExpiredAt.ShouldBe(new DateTimeOffset(2026, 10, 1, 10, 0, 0, TimeSpan.Zero));
            result.Data.CompletedAt.ShouldBe(new DateTimeOffset(2026, 10, 1, 9, 30, 0, TimeSpan.Zero));
        }

        [Fact]
        public async Task CreateSessionAsync_WithRequiredCustomerDetails_RequiredCustomerDetailsAreSerialized() {
            // Given: We create a session with required customer details
            var sessionRequest = new SessionRequest {
                Amount = new Amount(Currency.EUR, 10m),
                Description = "Description",
                RedirectUrl = DefaultRedirectUrl,
                RequiredCustomerDetails = [SessionRequiredCustomerDetail.BillingAddress, SessionRequiredCustomerDetail.ShippingAddress]
            };
            const string expectedPartialContent = @"""requiredCustomerDetails"":[""billing-address"",""shipping-address""]";
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Post,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}sessions",
                DefaultSessionJsonResponse,
                expectedPartialContent);
            using var sessionClient = new SessionClient("abcde", mockHttp.ToHttpClient());

            // When: We send the request
            var result = await sessionClient.CreateSessionAsync(sessionRequest);

            // Then: The required customer details are sent to Mollie
            mockHttp.VerifyNoOutstandingExpectation();
            result.Success.ShouldBeTrue();
        }
    }
}
