using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Shipment;
using Mollie.Api.Models.Subscription;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client {
    public class SubscriptionClientTests : BaseClientTests {
        [TestCase(null, null, null,false, "")]
        [TestCase("from", null, null, false, "?from=from")]
        [TestCase("from", 50, null, false, "?from=from&limit=50")]
        [TestCase(null, null,null, true, "?testmode=true")]
        [TestCase(null, null,"profile-id", true, "?profileId=profile-id")]
        public async Task GetSubscriptionListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, string profileId, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            const string customerId = "customer-id";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}customers/customer-id/subscriptions{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionListAsync(customerId, from, limit, profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [TestCase(null, null, null,false, "")]
        [TestCase("from", null, null, false, "?from=from")]
        [TestCase("from", 50, null, false, "?from=from&limit=50")]
        [TestCase(null, null,null, true, "?testmode=true")]
        [TestCase(null, null,"profile-id", true, "?profileId=profile-id")]
        public async Task GetAllSubscriptionList_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, string profileId, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}subscriptions{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetAllSubscriptionList(from, limit, profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [TestCase("customers/customer-id/subscriptions/subscription-id", false)]
        [TestCase("customers/customer-id/subscriptions/subscription-id?testmode=true", true)]
        public async Task GetSubscriptionAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool testModeParameter) {
            // Given: We retrieve a subscriptions
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}{expectedUrl}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionAsync(customerId, subscriptionId, testModeParameter);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [Test]
        public async Task RevokeMandate_TestmodeIsTrue_RequestContainsTestmodeModel() {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";
            
            string expectedContent = "\"testmode\":true";
            var mockHttp = this.CreateMockHttpMessageHandler(
                HttpMethod.Delete, 
                $"{BaseMollieClient.ApiEndPoint}customers/{customerId}/subscriptions/{subscriptionId}", 
                DefaultSubscriptionJsonToReturn, 
                expectedContent);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            await subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId, true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [TestCase(null, null, false, "")]
        [TestCase("from", null, false, "?from=from")]
        [TestCase("from", 50, false, "?from=from&limit=50")]
        [TestCase(null, null,true, "?testmode=true")]
        public async Task GetSubscriptionPaymentListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of subscriptions
            const string customerId = "customer-id";
            const string subscriptionId = "subscription-id";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}customers/{customerId}/subscriptions/{subscriptionId}/payments{expectedQueryString}")
                .Respond("application/json", DefaultSubscriptionJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("abcde", httpClient);

            // When: We send the request
            var result = await subscriptionClient.GetSubscriptionPaymentListAsync(customerId, subscriptionId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetSubscriptionListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionListAsync(customerId));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionAsync(customerId, "subscription-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionAsync("customer-Id", subscriptionId));

            // Then
            Assert.AreEqual($"Required URL argument 'subscriptionId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CreateSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.CreateSubscriptionAsync(customerId, new SubscriptionRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CancelSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.CancelSubscriptionAsync(customerId, "subscription-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CancelSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.CancelSubscriptionAsync("customer-Id", subscriptionId));

            // Then
            Assert.AreEqual($"Required URL argument 'subscriptionId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UpdateSubscriptionAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.UpdateSubscriptionAsync(customerId, "subscription-id", new SubscriptionUpdateRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UpdateSubscriptionAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.UpdateSubscriptionAsync("customer-Id", subscriptionId, new SubscriptionUpdateRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'subscriptionId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UGetSubscriptionPaymentListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionPaymentListAsync(customerId, "subscription-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetSubscriptionPaymentListAsync_NoSubscriptionIdIsGiven_ArgumentExceptionIsThrown(string subscriptionId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SubscriptionClient subscriptionClient = new SubscriptionClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await subscriptionClient.GetSubscriptionPaymentListAsync("customer-Id", subscriptionId));

            // Then
            Assert.AreEqual($"Required URL argument 'subscriptionId' is null or empty", exception.Message); 
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