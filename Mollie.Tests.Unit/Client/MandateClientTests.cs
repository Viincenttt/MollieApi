using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Mandate;
using RichardSzalay.MockHttp;

/*
namespace Mollie.Tests.Unit.Client {
    public class MandateClientTests : BaseClientTests {
        [TestCase("customers/customer-id/mandates/mandate-id", false)]
        [TestCase("customers/customer-id/mandates/mandate-id?testmode=true", true)]
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
            Assert.IsNotNull(result);
        }
        
        [TestCase(null, null, false, "")]
        [TestCase("from", null, false, "?from=from")]
        [TestCase("from", 50, false, "?from=from&limit=50")]
        [TestCase(null, null, true, "?testmode=true")]
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
            Assert.IsNotNull(result);
        }
        
        [Test]
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
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetMandateAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateAsync(mandateId, "mandate-id"));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetMandateAsync_NoMandateIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateAsync("customer-id", mandateId));

            // Then
            Assert.AreEqual($"Required URL argument 'mandateId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetMandateListAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.GetMandateListAsync(mandateId));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void CreateMandateAsync_NoCustomerIdIsGiven_ArgumentExceptionIsThrown(string mandateId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            MandateClient mandateClient = new MandateClient("api-key", httpClient);

            // When: We send the request
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await mandateClient.CreateMandateAsync(mandateId, new MandateRequest()));

            // Then
            Assert.AreEqual($"Required URL argument 'customerId' is null or empty", exception.Message); 
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
}*/