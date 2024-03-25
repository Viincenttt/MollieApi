using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class MandateClientTests : BaseClientTests {
        [Theory]
        [InlineData("customers/customer-id/mandates/mandate-id", false)]
        [InlineData("customers/customer-id/mandates/mandate-id?testmode=true", true)]
        public async Task GetMandateAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string expectedUrl, bool testModeParameter) {
            // Given: We retrieve a mandate
            const string customerId = "customer-id";
            const string mandateId = "mandate-id";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}{expectedUrl}")
                .Respond("application/json", DefaultMandateJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("abcde", httpClient);

            // When: We send the request
            var result = await mandateClient.GetMandateAsync(customerId, mandateId, testModeParameter);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.Should().NotBeNull();
        }
        
        [Theory]
        [InlineData(null, null, false, "")]
        [InlineData("from", null, false, "?from=from")]
        [InlineData("from", 50, false, "?from=from&limit=50")]
        [InlineData(null, null, true, "?testmode=true")]
        public async Task GetMandateListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue(string from, int? limit, bool testmode, string expectedQueryString) {
            // Given: We retrieve a list of mandates
            const string customerId = "customer-id";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{BaseMollieClient.ApiEndPoint}customers/{customerId}/mandates{expectedQueryString}")
                .Respond("application/json", DefaultMandateJsonToReturn);
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("abcde", httpClient);

            // When: We send the request
            var result = await mandateClient.GetMandateListAsync(customerId, from, limit, testmode);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
            result.Should().NotBeNull();
        }
        
        [Fact]
        public async Task RevokeMandate_TestmodeIsTrue_RequestContainsTestmodeModel() {
            // Given: We make a request to retrieve a payment with embedded refunds
            const string customerId = "customer-id";
            const string mandateId = "mandate-id";
            string expectedContent = "\"testmode\":true";
            var mockHttp = this.CreateMockHttpMessageHandler(
                HttpMethod.Delete, 
                $"{BaseMollieClient.ApiEndPoint}customers/{customerId}/mandates/{mandateId}", 
                DefaultMandateJsonToReturn, 
                expectedContent);
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("abcde", httpClient);

            // When: We send the request
            await mandateClient.RevokeMandate(customerId, mandateId, true);

            // Then
            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetMandateAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateAsync(mandateId, "mandate-id"));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetMandateAsync_NoMandateIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateAsync("customer-id", mandateId));

            // Then
            exception.Message.Should().Be("Required URL argument 'mandateId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetMandateListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateListAsync(mandateId));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateMandateAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);
            MandateRequest mandateRequest = new MandateRequest {
                ConsumerName = "John Doe",
                Method = PaymentMethod.DirectDebit
            };

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.CreateMandateAsync(mandateId, mandateRequest));

            // Then
            exception.Message.Should().Be("Required URL argument 'customerId' is null or empty");
        }

        private const string DefaultMandateJsonToReturn = @"{
    ""resource"": ""mandate"",
    ""id"": ""mdt_h3gAaD5zP"",
    ""mode"": ""test"",
    ""status"": ""valid"",
    ""method"": ""directdebit"",
    ""details"": {
        ""consumerName"": ""John Doe"",
        ""consumerAccount"": ""NL55INGB0000000000"",
        ""consumerBic"": ""INGBNL2A""
    },
    ""mandateReference"": ""YOUR-COMPANY-MD1380"",
    ""signatureDate"": ""2018-05-07"",
    ""createdAt"": ""2018-05-07T10:49:08+00:00""    
}";
    }
}