using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Terminal;
using RichardSzalay.MockHttp;

namespace Mollie.Tests.Unit.Client; 

/*
public class TerminalClientTests : BaseClientTests {
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void GetTerminalAsync_NoTerminalIdIsGiven_ArgumentExceptionIsThrown(string terminalId) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("api-key", httpClient);

        // When
        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await terminalClient.GetTerminalAsync(terminalId));

        // Then
        Assert.AreEqual($"Required URL argument 'terminalId' is null or empty", exception.Message);
    }

    [Test]
    public async Task GetTerminalAsync_WithTerminalId_ResponseIsDeserializedInExpectedFormat() {
        // Given
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
        
        // When
        TerminalResponse response = await terminalClient.GetTerminalAsync(terminalId);
        
        // Then
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
    
    [TestCase(null, null, null, false, "")]
    [TestCase("from", null, null, false, "?from=from")]
    [TestCase("from", 50, null, false, "?from=from&limit=50")]
    [TestCase(null, null, "profile-id", false, "?profileId=profile-id")]
    [TestCase(null, null, "profile-id", true, "?profileId=profile-id&testmode=true")]
    public async Task GetTerminalListAsync_QueryParameterOptions_CorrectParametersAreAdded(string from, int? limit, string profileId, bool testmode, string expectedQueryString) {
        // Given
        string jsonToReturnInMockResponse = CreateTerminalListJsonResponse();
        var mockHttp = this.CreateMockHttpMessageHandler(
            HttpMethod.Get, 
            $"{BaseMollieClient.ApiEndPoint}terminals{expectedQueryString}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        await terminalClient.GetTerminalListAsync(from, limit, profileId, testmode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Test]
    public async Task GetTerminalListAsync_ResponseIsDeserializedInExpectedFormat() {
        // Given
        string jsonToReturnInMockResponse = CreateTerminalListJsonResponse();
        var mockHttp = this.CreateMockHttpMessageHandler(
            HttpMethod.Get, 
            $"{BaseMollieClient.ApiEndPoint}terminals",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);
        
        // When
        ListResponse<TerminalResponse> response = await terminalClient.GetTerminalListAsync();
        
        // Then
        Assert.AreEqual(1, response.Count);
        Assert.AreEqual(response.Count, response.Items.Count);
        Assert.NotNull(response.Links);
        Assert.NotNull(response.Links.Self.Href);
    }

    private string CreateTerminalListJsonResponse() {
        string terminalJson = CreateTerminalJsonResponse("terminal-id", "description", "serial", "brand", "model");
        
        return @$"{{
    ""count"": 1,
    ""_embedded"": {{
        ""terminals"": [
            {terminalJson}
        ]
    }},
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminalss?limit=5"",
            ""type"": ""application/hal+json""
        }},
        ""previous"": null,
        ""next"": {{
            ""href"": ""https://api.mollie.com/v2/terminals?from=term_7MgL4wea46qkRcoTZjWEH&limit=5"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/terminals-api/list-terminals"",
            ""type"": ""text/html""
        }}
    }}
}}";
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
}*/