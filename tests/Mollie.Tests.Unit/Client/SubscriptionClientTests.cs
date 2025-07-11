﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Subscription.Request;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class SubscriptionClientTests : BaseClientTests {
        [Theory]
        [InlineData(null, null, null,false, "")]
        [InlineData("from", null, null, false, "?from=from")]
        [InlineData("from", 50, null, false, "?from=from&limit=50")]
        [InlineData(null, null,null, true, "?testmode=true")]
        [InlineData(null, null,"profile-id", true, "?profileId=profile-id")]
        public async Task GetSubscriptionListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string? from, int? limit, string? profileId, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            const string customerId = "customer-id";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}customers/customer-id/subscriptions{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionListAsync(customerId, from, limit, profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(null, null, null,false, "")]
        [InlineData("from", null, null, false, "?from=from")]
        [InlineData("from", 50, null, false, "?from=from&limit=50")]
        [InlineData(null, null,null, true, "?testmode=true")]
        [InlineData(null, null,"profile-id", true, "?profileId=profile-id")]
        public async Task GetAllSubscriptionList_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string? from, int? limit, string? profileId, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}subscriptions{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetAllSubscriptionList(from, limit, profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("customers/customer-id/subscriptions/subscription-id", false)]
        [InlineData("customers/customer-id/subscriptions/subscription-id?testmode=true", true)]
        public async Task GetSubscriptionAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool testModeParameter) {
            // Given: We retrieve a subscriptions
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}{expectedUrl}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionAsync(customerId, subscriptionId, testModeParameter);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task RevokeMandate_TestmodeIsTrue_RequestContainsTestmodeModel() {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";

            string expectedContent = "\"testmode\":true";
            var mockHttp = CreateMockHttpMessageHandler(
                HttpMethod.Delete,
                $"{BaseMollieClient.DefaultBaseApiEndPoint}customers/{customerId}/subscriptions/{subscriptionId}",
                DefaultSubscriptionJsonToReturn,
                expectedContent);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            await subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId, true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(null, null, false, "")]
        [InlineData("from", null, false, "?from=from")]
        [InlineData("from", 50, false, "?from=from&limit=50")]
        [InlineData(null, null,true, "?testmode=true")]
        public async Task GetSubscriptionPaymentListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string? from, int? limit, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}customers/{customerId}/subscriptions/{subscriptionId}/payments{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionPaymentListAsync(customerId, subscriptionId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSubscriptionListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionListAsync(customerId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionAsync(customerId, "subscription-id"));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string? subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionAsync("customer-Id", subscriptionId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'subscriptionId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);
            var subscriptionRequest = new SubscriptionRequest {
                Amount = new Amount(Currency.EUR, "100.00"),
                Times = 5,
                Interval = "1 month",
                Description = $"Subscription {Guid.NewGuid()}", // Subscriptions must have a unique name
                WebhookUrl = "http://www.google.nl",
                StartDate = DateTime.Now.AddDays(1),
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
#pragma warning disable CS8604 // Possible null reference argument.
                await subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.CancelSubscriptionAsync(customerId, "subscription-id"));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string? subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.CancelSubscriptionAsync("customer-Id", subscriptionId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'subscriptionId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.UpdateSubscriptionAsync(customerId, "subscription-id", new SubscriptionUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string? subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.UpdateSubscriptionAsync("customer-Id", subscriptionId, new SubscriptionUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'subscriptionId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSubscriptionPaymentListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string? customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionPaymentListAsync(customerId, "subscription-id"));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'customerId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSubscriptionPaymentListAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string? subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            var subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionPaymentListAsync("customer-Id", subscriptionId));
#pragma warning restore CS8604 // Possible null reference argument.

            // Then
            exception.Message.ShouldBe("Required URL argument 'subscriptionId' is null or empty");
        }

        private const string DefaultSubscriptionJsonToReturn = @"{
    ""resource"": ""subscription"",
    ""id"": ""subscription-id"",
    ""mode"": ""live"",
    ""createdAt"": ""2016-06-01T12:23:34+00:00"",
    ""status"": ""active"",
    ""amount"": {
        ""value"": ""25.00"",
        ""currency"": ""EUR""
    },
    ""times"": 4,
    ""timesRemaining"": 4,
    ""interval"": ""3 months"",
    ""startDate"": ""2016-06-01"",
    ""nextPaymentDate"": ""2016-09-01"",
    ""description"": ""Quarterly payment"",
    ""method"": null,
    ""mandateId"": ""mdt_38HS4fsS"",
    ""webhookUrl"": ""https://webshop.example.org/payments/webhook"",
    ""metadata"": {
        ""plan"": ""small""
    }
}";
    }
}
