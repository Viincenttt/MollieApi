using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Payment.Request;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client {
    public class CustomerClientTests : BaseClientTests {
        [TestCase("customers/customer-id", false)]
        [TestCase("customers/customer-id?testmode=true", true)]
        public async Task GetCustomerAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool testModeParameter) {
            // Given: We retrieve a customer
            const string customerId = "customer-id";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}{expectedUrl}")
                .Respond("application/json", DefaultCustomerJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("abcde", httpClient);

            // When: We send the request
            CustomerResponse customerResponse = await customerClient.GetCustomerAsync(customerId, testModeParameter);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(customerResponse);
        }
        
        [TestCase(null, null, false, "")]
        [TestCase("from", null, false, "?from=from")]
        [TestCase("from", 50, false, "?from=from&limit=50")]
        [TestCase(null, null, true, "?testmode=true")]
        public async Task GetCustomerListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of customers
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}customers{expectedQueryString}")
                .Respond("application/json", DefaultCustomerJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("abcde", httpClient);

            // When: We send the request
            var result = await customerClient.GetCustomerListAsync(from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [TestCase(null, null, null, false, "")]
        [TestCase("from", null, null, false, "?from=from")]
        [TestCase("from", 50, null, false, "?from=from&limit=50")]
        [TestCase(null, null, null, true, "?testmode=true")]
        [TestCase(null, null, "profile-id", true, "?profileId=profile-id")]
        public async Task GetCustomerPaymentListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, string profileId, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of customers
            const string customerId = "customer-id";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}customers/{customerId}/payments{expectedQueryString}")
                .Respond("application/json", DefaultCustomerJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("abcde", httpClient);

            // When: We send the request
            var result = await customerClient.GetCustomerPaymentListAsync(customerId, from, limit, profileId, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(result);
        }
        
        [Test]
        public async Task DeleteCustomerAsync_TestmodeIsTrue_RequestContainsTestmodeModel() {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string customerId = "customer-id";
            string expectedContent = "\"testmode\":true";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Delete, $"{BaseMollieClient.ApiEndPoint}customers/{customerId}", DefaultCustomerJsonToReturn, expectedContent);
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("abcde", httpClient);

            // When: We send the request
            await customerClient.DeleteCustomerAsync(customerId, true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UpdateCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.UpdateCustomerAsync(customerId, new CustomerRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void DeleteCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.DeleteCustomerAsync(customerId));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.GetCustomerAsync(customerId));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetCustomerPaymentListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.GetCustomerPaymentListAsync(customerId));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CreateCustomerPayment_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.CreateCustomerPayment(customerId, new PaymentRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }

        private const string DefaultCustomerJsonToReturn = @"{
    ""resource"": ""customer"",
    ""id"": ""customer-id"",
    ""mode"": ""test"",
    ""name"": ""Customer A"",
    ""email"": ""customer@example.org"",
    ""locale"": ""nl_NL"",
    ""metadata"": null,
    ""createdAt"": ""2018-04-06T13:23:21.0Z""    
}";
    }
}