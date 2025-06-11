using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class TerminalClientTests : BaseClientTests {
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetTerminalAsync_NoTerminalIdIsGiven_ArgumentExceptionIsThrown(string? terminalId) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("api-key", httpClient);

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await terminalClient.GetTerminalAsync(terminalId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'terminalId' is null or empty");
    }

    [Fact]
    public async Task GetTerminalAsync_WithTerminalId_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string terminalId = "terminal-id";
        const string description = "terminal-description";
        const string serialNumber = "serial-number";
        const string brand = "brand";
        const string model = "model";
        string jsonToReturnInMockResponse = CreateTerminalJsonResponse(terminalId, description, serialNumber, brand, model);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        TerminalResponse response = await terminalClient.GetTerminalAsync(terminalId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.Id.ShouldBe(terminalId);
        response.Description.ShouldBe(description);
        response.SerialNumber.ShouldBe(serialNumber);
        response.Brand.ShouldBe(brand);
        response.Model.ShouldBe(model);
        response.Links.ShouldNotBeNull();
        response.Links.Self.ShouldNotBeNull();
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/terminals/{terminalId}");
        response.Links.Documentation.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(true, "?testmode=true")]
    [InlineData(false, "")]
    public async Task GetTerminalAsync_QueryParameterOptions_CorrectParametersAreAdded(bool testMode, string expectedQueryString) {
        // Given
        const string terminalId = "terminal-id";
        const string description = "terminal-description";
        const string serialNumber = "serial-number";
        const string brand = "brand";
        const string model = "model";
        string jsonToReturnInMockResponse = CreateTerminalJsonResponse(terminalId, description, serialNumber, brand, model);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}{expectedQueryString}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        await terminalClient.GetTerminalAsync(terminalId, testMode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Theory]
    [InlineData(null, null, null, false, "")]
    [InlineData("from", null, null, false, "?from=from")]
    [InlineData("from", 50, null, false, "?from=from&limit=50")]
    [InlineData(null, null, "profile-id", false, "?profileId=profile-id")]
    [InlineData(null, null, "profile-id", true, "?profileId=profile-id&testmode=true")]
    public async Task GetTerminalListAsync_QueryParameterOptions_CorrectParametersAreAdded(string? from, int? limit, string? profileId, bool testmode, string expectedQueryString) {
        // Given
        string jsonToReturnInMockResponse = CreateTerminalListJsonResponse();
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals{expectedQueryString}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        await terminalClient.GetTerminalListAsync(from, limit, profileId, testmode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Fact]
    public async Task GetTerminalListAsync_ResponseIsDeserializedInExpectedFormat() {
        // Given
        string jsonToReturnInMockResponse = CreateTerminalListJsonResponse();
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        ListResponse<TerminalResponse> response = await terminalClient.GetTerminalListAsync();

        // Then
        response.Count.ShouldBe(1);
        response.Items.Count.ShouldBe(response.Count);
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldNotBeNull();
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
}
