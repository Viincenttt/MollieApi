using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Payment.Request;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class CustomerClientTests : BaseClientTests {
        [Theory]
        [InlineData("customers/customer-id", false)]
        [InlineData("customers/customer-id?testmode=true", true)]
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
            customerResponse.Should().NotBeNull();
        }
        
        [Theory]
        [InlineData(null, null, false, "")]
        [InlineData("from", null, false, "?from=from")]
        [InlineData("from", 50, false, "?from=from&limit=50")]
        [InlineData(null, null, true, "?testmode=true")]
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
            result.Should().NotBeNull();
        }
        
        [Theory]
        [InlineData(null, null, null, false, "")]
        [InlineData("from", null, null, false, "?from=from")]
        [InlineData("from", 50, null, false, "?from=from&limit=50")]
        [InlineData(null, null, null, true, "?testmode=true")]
        [InlineData(null, null, "profile-id", true, "?profileId=profile-id")]
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
            result.Should().NotBeNull();
        }
        
        [Fact]
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
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task UpdateCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.UpdateCustomerAsync(customerId, new CustomerRequest()));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task DeleteCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.DeleteCustomerAsync(customerId));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetCustomerAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.GetCustomerAsync(customerId));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetCustomerPaymentListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.GetCustomerPaymentListAsync(customerId));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateCustomerPayment_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string customerId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            CustomerClient customerClient = new CustomerClient("api-key", httpClient);
            var paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, 100),
                Description = "Order #12345",
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await customerClient.CreateCustomerPayment(customerId, paymentRequest));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
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