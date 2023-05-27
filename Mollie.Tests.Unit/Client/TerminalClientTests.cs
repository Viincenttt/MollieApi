using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.Terminal;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client; 

public class TerminalClientTests : BaseClientTests {
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void GetTerminalAsync_NoOrderIdIsGiven_ArgumentExceptionIsThrown(string terminalId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("api-key", httpClient);

        // When: We send the request
        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await terminalClient.GetTerminalAsync(terminalId));

        // Then
        Assert.AreEqual($"Required URL argument 'terminalId' is null or empty", exception.Message);
    }

    [Test]
    public async Task GetTerminalAsync_WithOrderId_ResponseIsDeserializedInExpectedFormat() {
        // Arrange
        const string terminalId = "terminal-id";
        const string description = "terminal-description";
        const string serialNumber = "serial-number";
        const string brand = "brand";
        const string model = "model";
        string jsonToReturnInMockResponse = CreateTerminalJsonResponse(terminalId, description, serialNumber, brand, model);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}terminals/{terminalId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);
        
        // Act
        TerminalResponse response = await terminalClient.GetTerminalAsync(terminalId);
        
        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        Assert.AreEqual(terminalId, response.Id);
        Assert.AreEqual(description, response.Description);
        Assert.AreEqual(serialNumber, response.SerialNumber);
        Assert.AreEqual(brand, response.Brand);
        Assert.AreEqual(model, response.Model);
        Assert.NotNull(response.Links);
        Assert.NotNull(response.Links.Self);
        Assert.AreEqual($"https://api.mollie.com/v2/terminals/{terminalId}", response.Links.Self.Href);
        Assert.NotNull(response.Links.Documentation);
    }

    private string CreateTerminalJsonResponse(string terminalId, string description, string serialNumber, string brand, string model) {
        return $@"{{
    ""id"": ""{terminalId}"",
    ""profileId"": ""pfl_QkEhN94Ba"",
    ""status"": ""active"",
    ""brand"": ""{brand}"",
    ""model"": ""{model}"",
    ""serialNumber"": ""{serialNumber}"",
    ""currency"": ""EUR"",
    ""description"": ""{description}"",
    ""createdAt"": ""2022-02-12T11:58:35.0Z"",
    ""updatedAt"": ""2022-11-15T13:32:11+00:00"",
    ""deactivatedAt"": ""2022-02-12T12:13:35.0Z"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/{terminalId}"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/terminals-api/get-terminal"",
            ""type"": ""text/html""
        }}
    }}
}}";
    }
}