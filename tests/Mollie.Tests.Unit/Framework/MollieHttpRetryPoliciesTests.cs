using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Response;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Framework;

public class MollieHttpRetryPoliciesTests {
    private const string PaymentId = "tr_WDqYK6vllg";
    private const string PaymentJsonResponse = @"{
        ""resource"": ""payment"",
        ""id"": ""tr_WDqYK6vllg"",
        ""mode"": ""test"",
        ""status"": ""open"",
        ""createdAt"": ""2024-01-01T00:00:00Z"",
        ""amount"": { ""currency"": ""EUR"", ""value"": ""10.00"" },
        ""description"": ""Order #12345"",
        ""profileId"": ""pfl_QkEhN94Ba"",
        ""sequenceType"": ""oneoff"",
        ""_links"": { ""self"": { ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"", ""type"": ""application/hal+json"" } }
    }";

    [Fact]
    public async Task TransientHttpErrorRetryPolicy_TransientErrorsFollowedBySuccess_RequestIsRetried() {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        string url = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{PaymentId}";
        mockHttp.Expect(HttpMethod.Get, url).Respond(_ => CreateTransientErrorResponse(HttpStatusCode.BadGateway));
        mockHttp.Expect(HttpMethod.Get, url).Respond(_ => CreateTransientErrorResponse(HttpStatusCode.ServiceUnavailable));
        mockHttp.Expect(HttpMethod.Get, url).Respond(MediaTypeNames.Application.Json, PaymentJsonResponse);
        var paymentClient = CreatePaymentClient(mockHttp);

        // When
        MollieResult<PaymentResponse> result = await paymentClient.GetPaymentAsync(PaymentId);

        // Then
        result.Success.ShouldBeTrue();
        result.Data.Id.ShouldBe(PaymentId);
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task TransientHttpErrorRetryPolicy_ClientError_RequestIsNotRetried() {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        string url = $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{PaymentId}";
        mockHttp.Expect(HttpMethod.Get, url).Respond(HttpStatusCode.NotFound, MediaTypeNames.Application.Json,
            @"{""status"":404,""title"":""Not Found"",""detail"":""No payment exists with token tr_WDqYK6vllg.""}");
        var paymentClient = CreatePaymentClient(mockHttp);

        // When
        MollieResult<PaymentResponse> result = await paymentClient.GetPaymentAsync(PaymentId);

        // Then
        result.Success.ShouldBeFalse();
        result.Error.Status.ShouldBe(404);
        mockHttp.VerifyNoOutstandingExpectation();
        mockHttp.GetMatchCount(mockHttp.Fallback).ShouldBe(0);
    }

    [Fact]
    public void GetRetryAfterDelay_WithDelta_ReturnsDelta() {
        // Given
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(5));

        // When
        TimeSpan? delay = MollieHttpRetryPolicies.GetRetryAfterDelay(response, DateTimeOffset.UtcNow);

        // Then
        delay.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void GetRetryAfterDelay_WithDate_ReturnsTimeUntilDate() {
        // Given
        var now = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(now.AddSeconds(30));

        // When
        TimeSpan? delay = MollieHttpRetryPolicies.GetRetryAfterDelay(response, now);

        // Then
        delay.ShouldBe(TimeSpan.FromSeconds(30));
    }

    [Fact]
    public void GetRetryAfterDelay_WithDateInThePast_ReturnsZero() {
        // Given
        var now = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(now.AddSeconds(-30));

        // When
        TimeSpan? delay = MollieHttpRetryPolicies.GetRetryAfterDelay(response, now);

        // Then
        delay.ShouldBe(TimeSpan.Zero);
    }

    [Fact]
    public void GetRetryAfterDelay_WithoutHeader_ReturnsNull() {
        // Given
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

        // When
        TimeSpan? delay = MollieHttpRetryPolicies.GetRetryAfterDelay(response, DateTimeOffset.UtcNow);

        // Then
        delay.ShouldBeNull();
    }

    private static IPaymentClient CreatePaymentClient(MockHttpMessageHandler mockHttp) {
        var services = new ServiceCollection();
        services.AddMollieApi(options => {
            options.ApiKey = "test_api_key";
            options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
        });
        services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(() => mockHttp));
        return services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<IPaymentClient>();
    }

    private static HttpResponseMessage CreateTransientErrorResponse(HttpStatusCode statusCode) {
        // A Retry-After of zero keeps the test fast and verifies that the header is honoured
        var response = new HttpResponseMessage(statusCode) {
            Content = new StringContent(
                $@"{{""status"":{(int)statusCode},""title"":""Error"",""detail"":""Transient error""}}",
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.Zero);
        return response;
    }
}
