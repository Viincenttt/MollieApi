using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Request;
using Mollie.Api.Models.Terminal.Response;
using RichardSzalay.MockHttp;
using Xunit;
using SortDirection = Mollie.Api.Models.SortDirection;

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
        var result = await terminalClient.GetTerminalAsync(terminalId);
        TerminalResponse response = result.Data!;

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
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
        var result = await terminalClient.GetTerminalListAsync();
        ListResponse<TerminalResponse> response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.Count.ShouldBe(1);
        response.Items.Count.ShouldBe(response.Count);
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldNotBeNull();
    }

    [Fact]
    public async Task CreateTerminalPairingCodeAsync_WithIncludeQrCode_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string pairingCodeId = "termpc_R7gX5Ea9bC4DkFj3G";
        var request = new TerminalPairingCodeRequest {
            ProfileId = "pfl_jA9bC4DkFj3G"
        };
        string jsonToReturnInMockResponse = CreateTerminalPairingCodeJsonResponse(pairingCodeId, TerminalPairingCodeStatus.Active, "null", PairingCodeQrCodeJson);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/pairing-codes?include=details.qrCode",
            jsonToReturnInMockResponse,
            "{\"profileId\":\"pfl_jA9bC4DkFj3G\"}");
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        var result = await terminalClient.CreateTerminalPairingCodeAsync(request, includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        TerminalPairingCodeResponse response = result.Data;
        response.Resource.ShouldBe("terminal-pairing-code");
        response.Id.ShouldBe(pairingCodeId);
        response.Mode.ShouldBe(Mode.Live);
        response.Code.ShouldBe("20eb5ca1f78b48ae9e2b");
        response.ProfileId.ShouldBe("pfl_jA9bC4DkFj3G");
        response.Status.ShouldBe(TerminalPairingCodeStatus.Active);
        response.Details.ShouldNotBeNull();
        response.Details.QrCode.ShouldNotBeNull();
        response.Details.QrCode.Height.ShouldBe(256);
        response.Details.QrCode.Width.ShouldBe(256);
        response.Details.QrCode.Src.ShouldBe("data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4w");
        response.ExpiresAt.ShouldBe(new DateTimeOffset(2026, 3, 10, 10, 0, 0, TimeSpan.Zero));
        response.RevokedAt.ShouldBeNull();
        response.CreatedAt.ShouldBe(new DateTimeOffset(2025, 12, 10, 10, 0, 0, TimeSpan.Zero));
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/terminals/pairing-codes/{pairingCodeId}");
        response.Links.Profile.Href.ShouldBe("https://api.mollie.com/v2/profiles/pfl_jA9bC4DkFj3G");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/terminals-get-pairing-code");
    }

    [Theory]
    [InlineData(null, null, null, null, "")]
    [InlineData("termpc_123", 50, null, null, "?from=termpc_123&limit=50")]
    [InlineData(null, null, "pfl_QkEhN94Ba", null, "?profileId=pfl_QkEhN94Ba")]
    [InlineData(null, null, null, SortDirection.Asc, "?sort=asc")]
    public async Task GetTerminalPairingCodeListAsync_QueryParameterOptions_CorrectParametersAreAdded(
        string? from, int? limit, string? profileId, SortDirection? sort, string expectedQueryString) {
        // Given
        string jsonToReturnInMockResponse = CreateTerminalPairingCodeListJsonResponse();
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/pairing-codes{expectedQueryString}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        var result = await terminalClient.GetTerminalPairingCodeListAsync(from, limit, profileId, sort);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        result.Data.Count.ShouldBe(1);
        result.Data.Items.Count.ShouldBe(1);
        result.Data.Items[0].Id.ShouldBe("termpc_R7gX5Ea9bC4DkFj3G");
    }

    [Theory]
    [InlineData(false, "")]
    [InlineData(true, "?include=details.qrCode")]
    public async Task GetTerminalPairingCodeAsync_WithIncludeQrCode_CorrectParametersAreAdded(bool includeQrCode, string expectedQueryString) {
        // Given
        const string pairingCodeId = "termpc_R7gX5Ea9bC4DkFj3G";
        string jsonToReturnInMockResponse = CreateTerminalPairingCodeJsonResponse(pairingCodeId, TerminalPairingCodeStatus.Active, "null");
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/pairing-codes/{pairingCodeId}{expectedQueryString}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        var result = await terminalClient.GetTerminalPairingCodeAsync(pairingCodeId, includeQrCode);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        result.Data.Id.ShouldBe(pairingCodeId);
        result.Data.Details.ShouldBeNull();
    }

    [Fact]
    public async Task RevokeTerminalPairingCodeAsync_WithPairingCodeId_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string pairingCodeId = "termpc_R7gX5Ea9bC4DkFj3G";
        string jsonToReturnInMockResponse = CreateTerminalPairingCodeJsonResponse(pairingCodeId, TerminalPairingCodeStatus.Revoked, "\"2025-12-10T10:03:23+00:00\"");
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Delete,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}terminals/pairing-codes/{pairingCodeId}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("abcde", httpClient);

        // When
        var result = await terminalClient.RevokeTerminalPairingCodeAsync(pairingCodeId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        result.Data.Status.ShouldBe(TerminalPairingCodeStatus.Revoked);
        result.Data.RevokedAt.ShouldBe(new DateTimeOffset(2025, 12, 10, 10, 3, 23, TimeSpan.Zero));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task RevokeTerminalPairingCodeAsync_NoPairingCodeIdIsGiven_ArgumentExceptionIsThrown(string? pairingCodeId) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var terminalClient = new TerminalClient("api-key", httpClient);

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await terminalClient.RevokeTerminalPairingCodeAsync(pairingCodeId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'pairingCodeId' is null or empty");
    }

    private const string PairingCodeQrCodeJson = @"{
        ""qrCode"": {
            ""height"": 256,
            ""width"": 256,
            ""src"": ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4w""
        }
    }";

    private string CreateTerminalPairingCodeListJsonResponse() {
        string pairingCodeJson = CreateTerminalPairingCodeJsonResponse("termpc_R7gX5Ea9bC4DkFj3G", TerminalPairingCodeStatus.Active, "null");

        return @$"{{
    ""count"": 1,
    ""_embedded"": {{
        ""terminal-pairing-codes"": [
            {pairingCodeJson}
        ]
    }},
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/pairing-codes"",
            ""type"": ""application/hal+json""
        }},
        ""previous"": null,
        ""next"": null,
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/terminals-list-pairing-codes"",
            ""type"": ""text/html""
        }}
    }}
}}";
    }

    private string CreateTerminalPairingCodeJsonResponse(string pairingCodeId, string status, string revokedAt, string details = "null") {
        return $@"{{
    ""resource"": ""terminal-pairing-code"",
    ""id"": ""{pairingCodeId}"",
    ""mode"": ""live"",
    ""code"": ""20eb5ca1f78b48ae9e2b"",
    ""profileId"": ""pfl_jA9bC4DkFj3G"",
    ""status"": ""{status}"",
    ""details"": {details},
    ""expiresAt"": ""2026-03-10T10:00:00+00:00"",
    ""revokedAt"": {revokedAt},
    ""createdAt"": ""2025-12-10T10:00:00+00:00"",
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/terminals/pairing-codes/{pairingCodeId}"",
            ""type"": ""application/hal+json""
        }},
        ""profile"": {{
            ""href"": ""https://api.mollie.com/v2/profiles/pfl_jA9bC4DkFj3G"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/terminals-get-pairing-code"",
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
