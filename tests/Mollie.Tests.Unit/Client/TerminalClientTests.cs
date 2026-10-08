using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Framework.Authentication;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.UnreferencedRefund.Request;
using Mollie.Api.Models.UnreferencedRefund.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;
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

    [Fact]
    public async Task CreateUnreferencedRefundAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string terminalId = "term_7MgL4wea46qkRcoTZjWEH";
        const string unreferencedRefundId = "unref_vytxeTZskVKR7C7WgdSP3d";
        var request = new UnreferencedRefundRequest {
            Description = "Refund of a pair of jeans",
            Amount = new Amount(Currency.EUR, 20.00m)
        };
        const string expectedRequestContent = @"{""description"":""Refund of a pair of jeans"",""amount"":{""currency"":""EUR"",""value"":""20.00""}}";
        string jsonResponse = CreateUnreferencedRefundJsonResponse(terminalId, unreferencedRefundId);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}/unreferenced-refunds")
            .With(httpRequest => httpRequest.Content!.ReadAsStringAsync().Result == expectedRequestContent)
            .Respond(HttpStatusCode.Created, "application/json", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        UnreferencedRefundResponse response = await terminalClient.CreateUnreferencedRefundAsync(terminalId, request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        AssertUnreferencedRefundResponse(response, terminalId, unreferencedRefundId);
    }

    [Fact]
    public async Task CreateUnreferencedRefundAsync_WithMetadata_MetadataIsSerializedAsRawJson() {
        // Given
        const string terminalId = "term_7MgL4wea46qkRcoTZjWEH";
        var request = new UnreferencedRefundRequest {
            Description = "Refund of a pair of jeans",
            Amount = new Amount(Currency.EUR, 20.00m),
            Metadata = @"{""order_id"":""12345""}"
        };
        const string expectedRequestContent = @"{""description"":""Refund of a pair of jeans"",""amount"":{""currency"":""EUR"",""value"":""20.00""},""metadata"":{""order_id"":""12345""}}";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}/unreferenced-refunds")
            .With(httpRequest => httpRequest.Content!.ReadAsStringAsync().Result == expectedRequestContent)
            .Respond(HttpStatusCode.Created, "application/json", CreateUnreferencedRefundJsonResponse(terminalId, "unref_123"));
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        await terminalClient.CreateUnreferencedRefundAsync(terminalId, request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CreateUnreferencedRefundAsync_WithProfileIdAndOauthToken_ProfileIdIsSerialized() {
        // Given
        const string terminalId = "term_7MgL4wea46qkRcoTZjWEH";
        const string profileId = "pfl_QkEhN94Ba";
        var request = new UnreferencedRefundRequest {
            Description = "Refund of a pair of jeans",
            Amount = new Amount(Currency.EUR, 20.00m),
            ProfileId = profileId
        };
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}/unreferenced-refunds",
            CreateUnreferencedRefundJsonResponse(terminalId, "unref_123"),
            expectedPartialContent: $@"""profileId"":""{profileId}""");
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("access_token", httpClient);

        // When
        await terminalClient.CreateUnreferencedRefundAsync(terminalId, request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CreateUnreferencedRefundAsync_WithProfileIdAndApiKey_InvalidOperationExceptionIsThrown() {
        // Given
        var request = new UnreferencedRefundRequest {
            Description = "Refund of a pair of jeans",
            Amount = new Amount(Currency.EUR, 20.00m),
            ProfileId = "pfl_QkEhN94Ba"
        };
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => terminalClient.CreateUnreferencedRefundAsync("term_7MgL4wea46qkRcoTZjWEH", request));

        // Then
        exception.Message.ShouldBe("The provided token isn't an oauth token. Are you trying to use oauth specific parameters such as ProfileId or TestMode using an API key?");
        mockHttp.GetMatchCount(mockHttp.Fallback).ShouldBe(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task CreateUnreferencedRefundAsync_NoTerminalIdIsGiven_ArgumentExceptionIsThrown(string? terminalId) {
        // Given
        var request = new UnreferencedRefundRequest {
            Description = "Refund of a pair of jeans",
            Amount = new Amount(Currency.EUR, 20.00m)
        };
        var terminalClient = new TerminalClient("test_api_key", new MockHttpMessageHandler().ToHttpClient());

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => terminalClient.CreateUnreferencedRefundAsync(terminalId, request));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'terminalId' is null or empty");
    }

    [Fact]
    public async Task GetUnreferencedRefundAsync_WithIds_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string terminalId = "term_7MgL4wea46qkRcoTZjWEH";
        const string unreferencedRefundId = "unref_vytxeTZskVKR7C7WgdSP3d";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}/unreferenced-refunds/{unreferencedRefundId}",
            CreateUnreferencedRefundJsonResponse(terminalId, unreferencedRefundId));
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        UnreferencedRefundResponse response = await terminalClient.GetUnreferencedRefundAsync(terminalId, unreferencedRefundId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        AssertUnreferencedRefundResponse(response, terminalId, unreferencedRefundId);
    }

    [Fact]
    public async Task GetUnreferencedRefundAsync_WithUrlObjectLink_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string terminalId = "term_7MgL4wea46qkRcoTZjWEH";
        const string unreferencedRefundId = "unref_vytxeTZskVKR7C7WgdSP3d";
        string url = $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/{terminalId}/unreferenced-refunds/{unreferencedRefundId}";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            url,
            CreateUnreferencedRefundJsonResponse(terminalId, unreferencedRefundId));
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);
        var urlObject = new UrlObjectLink<UnreferencedRefundResponse> {
            Href = url,
            Type = "application/hal+json"
        };

        // When
        UnreferencedRefundResponse response = await terminalClient.GetUnreferencedRefundAsync(urlObject);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.Id.ShouldBe(unreferencedRefundId);
    }

    [Theory]
    [InlineData("", "unref_123", "terminalId")]
    [InlineData(" ", "unref_123", "terminalId")]
    [InlineData(null, "unref_123", "terminalId")]
    [InlineData("term_123", "", "unreferencedRefundId")]
    [InlineData("term_123", " ", "unreferencedRefundId")]
    [InlineData("term_123", null, "unreferencedRefundId")]
    public async Task GetUnreferencedRefundAsync_NoIdIsGiven_ArgumentExceptionIsThrown(
        string? terminalId, string? unreferencedRefundId, string expectedParameterName) {
        // Given
        var terminalClient = new TerminalClient("test_api_key", new MockHttpMessageHandler().ToHttpClient());

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => terminalClient.GetUnreferencedRefundAsync(terminalId, unreferencedRefundId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe($"Required URL argument '{expectedParameterName}' is null or empty");
    }

    [Theory]
    [InlineData(null, null, "")]
    [InlineData("unref_123", null, "?from=unref_123")]
    [InlineData(null, 50, "?limit=50")]
    [InlineData("unref_123", 50, "?from=unref_123&limit=50")]
    public async Task GetUnreferencedRefundListAsync_QueryParameterOptions_CorrectParametersAreAdded(
        string? from, int? limit, string expectedQueryString) {
        // Given
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/unreferenced-refunds{expectedQueryString}",
            CreateUnreferencedRefundListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        await terminalClient.GetUnreferencedRefundListAsync(from, limit);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetUnreferencedRefundListAsync_WithProfileIdAndOauthToken_ProfileIdIsAddedToQueryString() {
        // Given
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/unreferenced-refunds?profileId=pfl_QkEhN94Ba",
            CreateUnreferencedRefundListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("access_token", httpClient);

        // When
        await terminalClient.GetUnreferencedRefundListAsync(profileId: "pfl_QkEhN94Ba");

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetUnreferencedRefundListAsync_WithProfileIdAndApiKey_InvalidOperationExceptionIsThrown() {
        // Given
        var terminalClient = new TerminalClient("test_api_key", new MockHttpMessageHandler().ToHttpClient());

        // When
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => terminalClient.GetUnreferencedRefundListAsync(profileId: "pfl_QkEhN94Ba"));

        // Then
        exception.Message.ShouldBe("The provided token isn't an oauth token. Are you trying to use oauth specific parameters such as ProfileId or TestMode using an API key?");
    }

    [Fact]
    public async Task GetUnreferencedRefundListAsync_TestmodeIsEnabledInOptions_TestmodeIsNotAddedToQueryString() {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/unreferenced-refunds")
            .With(request => request.RequestUri!.Query == string.Empty)
            .Respond("application/json", CreateUnreferencedRefundListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var mollieClientOptions = new MollieClientOptions {
            ApiKey = "access_token",
            Testmode = true
        };
        var secretManager = new DefaultMollieSecretManager(mollieClientOptions.ApiKey);
        var terminalClient = new TerminalClient(mollieClientOptions, secretManager, httpClient);

        // When
        await terminalClient.GetUnreferencedRefundListAsync();

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetUnreferencedRefundListAsync_ResponseIsDeserializedInExpectedFormat() {
        // Given
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/unreferenced-refunds",
            CreateUnreferencedRefundListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);

        // When
        ListResponse<UnreferencedRefundResponse> response = await terminalClient.GetUnreferencedRefundListAsync();

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.Count.ShouldBe(1);
        response.Items.Count.ShouldBe(response.Count);
        AssertUnreferencedRefundResponse(response.Items[0], "term_7MgL4wea46qkRcoTZjWEH", "unref_vytxeTZskVKR7C7WgdSP3d");
        response.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/terminals/unreferenced-refunds?limit=5");
        response.Links.Previous.ShouldBeNull();
        response.Links.Next.ShouldNotBeNull();
        response.Links.Next.Href.ShouldBe("https://api.mollie.com/v2/terminals/unreferenced-refunds?from=unref_4xNmEv8WWkrp7mjvPmq3cx&limit=5");
    }

    [Fact]
    public async Task GetUnreferencedRefundListAsync_WithUrlObjectLink_ResponseIsDeserializedInExpectedFormat() {
        // Given
        string url = $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/unreferenced-refunds?from=unref_4xNmEv8WWkrp7mjvPmq3cx&limit=5";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            url,
            CreateUnreferencedRefundListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("test_api_key", httpClient);
        var urlObject = new UrlObjectLink<ListResponse<UnreferencedRefundResponse>> {
            Href = url,
            Type = "application/hal+json"
        };

        // When
        ListResponse<UnreferencedRefundResponse> response = await terminalClient.GetUnreferencedRefundListAsync(urlObject);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.Items.Count.ShouldBe(1);
    }

    private static void AssertUnreferencedRefundResponse(UnreferencedRefundResponse response, string terminalId, string unreferencedRefundId) {
        response.Resource.ShouldBe("unreferenced-refund");
        response.Id.ShouldBe(unreferencedRefundId);
        response.Mode.ShouldBe(Mode.Live);
        response.Description.ShouldBe("Refund of a pair of jeans");
        response.Amount.Currency.ShouldBe(Currency.EUR);
        response.Amount.Value.ShouldBe("20.00");
        response.Status.ShouldBe(UnreferencedRefundStatus.Open);
        response.TerminalId.ShouldBe(terminalId);
        response.Metadata.ShouldBe(@"{""order_id"":""12345""}");
        response.CreatedAt.ToUniversalTime().ShouldBe(new DateTime(2023, 3, 14, 17, 9, 2, DateTimeKind.Utc));
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/terminals/{terminalId}/unreferenced-refunds/{unreferencedRefundId}");
        response.Links.Terminal.ShouldNotBeNull();
        response.Links.Terminal.Href.ShouldBe($"https://api.mollie.com/v2/terminals/{terminalId}");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/create-unreferenced-refund");
    }

    private string CreateUnreferencedRefundListJsonResponse() {
        string unreferencedRefundJson = CreateUnreferencedRefundJsonResponse("term_7MgL4wea46qkRcoTZjWEH", "unref_vytxeTZskVKR7C7WgdSP3d");

        return @$"{{
    ""count"": 1,
    ""_embedded"": {{
        ""unreferenced-refunds"": [
            {unreferencedRefundJson}
        ]
    }},
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/unreferenced-refunds?limit=5"",
            ""type"": ""application/hal+json""
        }},
        ""previous"": null,
        ""next"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/unreferenced-refunds?from=unref_4xNmEv8WWkrp7mjvPmq3cx&limit=5"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/list-unreferenced-refunds"",
            ""type"": ""text/html""
        }}
    }}
}}";
    }

    private string CreateUnreferencedRefundJsonResponse(string terminalId, string unreferencedRefundId) {
        return $@"{{
    ""resource"": ""unreferenced-refund"",
    ""id"": ""{unreferencedRefundId}"",
    ""mode"": ""live"",
    ""description"": ""Refund of a pair of jeans"",
    ""amount"": {{
        ""currency"": ""EUR"",
        ""value"": ""20.00""
    }},
    ""status"": ""open"",
    ""terminalId"": ""{terminalId}"",
    ""metadata"": {{""order_id"":""12345""}},
    ""createdAt"": ""2023-03-14T17:09:02+00:00"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/{terminalId}/unreferenced-refunds/{unreferencedRefundId}"",
            ""type"": ""application/hal+json""
        }},
        ""terminal"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/{terminalId}"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/create-unreferenced-refund"",
            ""type"": ""text/html""
        }}
    }}
}}";
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
