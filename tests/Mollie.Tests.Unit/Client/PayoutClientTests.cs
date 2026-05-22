using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payout.Request;
using Mollie.Api.Models.Payout.Response;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Unit.Client;

public class PayoutClientTests : BaseClientTests {
    [Fact]
    public async Task CreatePayoutAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
        // Given: we create a payout request with required parameters
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        PayoutRequest request = new() {
            BalanceId = balanceId
        };
        string jsonToReturnInMockResponse = CreatePayoutJsonResponse(payoutId, balanceId, null);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When: We send the request
        PayoutResponse response = await client.CreatePayoutAsync(request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Resource.ShouldBe("payout");
        response.Id.ShouldBe(payoutId);
        response.BalanceId.ShouldBe(balanceId);
        response.Status.ShouldBe(PayoutStatus.Requested);
        response.StatusReason.Code.ShouldBe("requested");
        response.StatusReason.Message.ShouldBe("The payout has been requested.");
        response.CreatedAt.ToUniversalTime().ShouldBe(new DateTime(2024, 3, 20, 9, 13, 37, DateTimeKind.Utc));
        response.InitiatedAt.ShouldBeNull();
        response.CompletedAt.ShouldBeNull();
        response.CanceledAt.ShouldBeNull();
        response.Mode.ShouldBe(Mode.Live);
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/payouts/{payoutId}");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/get-payout");
    }

    [Fact]
    public async Task CreatePayoutAsync_WithAmountAndDescription_ResponseIsDeserializedInExpectedFormat() {
        // Given: we create a payout request with amount and description
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        var amount = new Amount(Currency.EUR, "10.00");
        PayoutRequest request = new() {
            BalanceId = balanceId,
            Amount = amount,
            Description = "My payout description"
        };
        string jsonToReturnInMockResponse = CreatePayoutJsonResponse(payoutId, balanceId, amount, request.Description);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When: We send the request
        PayoutResponse response = await client.CreatePayoutAsync(request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Amount.ShouldNotBeNull();
        response.Amount!.Currency.ShouldBe("EUR");
        response.Amount.Value.ShouldBe("10.00");
        response.Description.ShouldBe("My payout description");
    }

    [Theory]
    [InlineData(null, null, null, false, null, "")]
    [InlineData(null, "from", null, false, null, "?from=from")]
    [InlineData(null, "from", 50, false, null, "?from=from&limit=50")]
    [InlineData(null, null, null, true, null, "?testmode=true")]
    [InlineData(null, null, null, true, SortDirection.Desc, "?testmode=true&sort=desc")]
    [InlineData(null, null, null, true, SortDirection.Asc, "?testmode=true&sort=asc")]
    [InlineData("bal_gVMhHKqSSRYJyPsuoPNFH", null, null, false, null, "?balanceId=bal_gVMhHKqSSRYJyPsuoPNFH")]
    [InlineData("bal_gVMhHKqSSRYJyPsuoPNFH", null, null, true, SortDirection.Desc, "?testmode=true&sort=desc&balanceId=bal_gVMhHKqSSRYJyPsuoPNFH")]
    public async Task GetPayoutListAsync_QueryStringParameters_AreFormattedCorrectly(
        string? balanceId, string? from, int? limit, bool testmode, SortDirection? sort, string expectedQueryString) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}payouts{expectedQueryString}")
            .Respond("application/json", CreatePayoutListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        var result = await client.GetPayoutListAsync(balanceId, from, limit, sort, testmode);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task CreatePayoutAsync_WithTestmode_SendsTestmodeInRequest() {
        // Given
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        PayoutRequest request = new() {
            BalanceId = balanceId,
            Testmode = true
        };
        string jsonToReturnInMockResponse = CreatePayoutJsonResponse(payoutId, balanceId, null);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts",
            jsonToReturnInMockResponse,
            expectedPartialContent: "\"testmode\":true");
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        PayoutResponse response = await client.CreatePayoutAsync(request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetPayoutAsync_WithValidPayoutId_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        string jsonToReturnInMockResponse = CreatePayoutJsonResponse(payoutId, balanceId, null);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts/{payoutId}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        PayoutResponse response = await client.GetPayoutAsync(payoutId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Id.ShouldBe(payoutId);
        response.BalanceId.ShouldBe(balanceId);
        response.Status.ShouldBe(PayoutStatus.Requested);
    }

    [Fact]
    public async Task GetPayoutAsync_WithTestmodeTrue_QueryStringContainsTestmodeParameter() {
        // Given
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        string jsonToReturnInMockResponse = CreatePayoutJsonResponse(payoutId, balanceId, null);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts/{payoutId}?testmode=true",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        PayoutResponse response = await client.GetPayoutAsync(payoutId, testmode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Id.ShouldBe(payoutId);
    }

    [Fact]
    public async Task GetPayoutAsync_WithEmptyPayoutId_ThrowsException() {
        // Given
        var client = new PayoutClient("abcde", new HttpClient());

        // When / Then
        var exception = await Should.ThrowAsync<ArgumentException>(async () => {
            await client.GetPayoutAsync("");
        });
        exception.Message.ShouldContain("payoutId");
    }

    [Fact]
    public async Task CancelPayoutAsync_WithValidPayoutId_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        string jsonToReturnInMockResponse = CreateCanceledPayoutJsonResponse(payoutId, balanceId);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Delete,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts/{payoutId}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        PayoutResponse response = await client.CancelPayoutAsync(payoutId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Id.ShouldBe(payoutId);
        response.BalanceId.ShouldBe(balanceId);
        response.Status.ShouldBe(PayoutStatus.Canceled);
        response.CanceledAt.ShouldNotBeNull();
    }

    [Fact]
    public async Task CancelPayoutAsync_WithTestmodeTrue_SendsTestmodeInRequestBody() {
        // Given
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        const string balanceId = "bal_gVMhHKqSSRYJyPsuoPNFH";
        string jsonToReturnInMockResponse = CreateCanceledPayoutJsonResponse(payoutId, balanceId);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Delete,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payouts/{payoutId}",
            jsonToReturnInMockResponse,
            expectedPartialContent: "\"testmode\":true");
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new PayoutClient("abcde", httpClient);

        // When
        PayoutResponse response = await client.CancelPayoutAsync(payoutId, testmode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
    }

    [Fact]
    public async Task CancelPayoutAsync_WithEmptyPayoutId_ThrowsException() {
        // Given
        var client = new PayoutClient("abcde", new HttpClient());

        // When / Then
        var exception = await Should.ThrowAsync<ArgumentException>(async () => {
            await client.CancelPayoutAsync("");
        });
        exception.Message.ShouldContain("payoutId");
    }

    private string CreateCanceledPayoutJsonResponse(string payoutId, string balanceId) {
        return $@"{{
  ""resource"": ""payout"",
  ""id"": ""{payoutId}"",
  ""balanceId"": ""{balanceId}"",
  ""amount"": {{""currency"": ""EUR"", ""value"": ""10.00""}},
  ""description"": ""My payout description"",
  ""status"": ""canceled"",
  ""statusReason"": {{
    ""code"": ""canceled"",
    ""message"": ""The payout has been canceled.""
  }},
  ""createdAt"": ""2024-03-20T09:13:37+00:00"",
  ""initiatedAt"": null,
  ""completedAt"": null,
  ""canceledAt"": ""2024-03-20T09:15:00+00:00"",
  ""mode"": ""live"",
  ""_links"": {{
    ""self"": {{
      ""href"": ""https://api.mollie.com/v2/payouts/{payoutId}"",
      ""type"": ""application/hal+json""
    }},
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/get-payout"",
      ""type"": ""text/html""
    }}
  }}
}}";
    }

    private string CreatePayoutListJsonResponse() {
        return $@"{{
  ""count"": 1,
  ""_embedded"": {{
    ""payouts"": [
      {CreatePayoutJsonResponse("payout_j8NvRAM2WNZtsykpLEX8J", "bal_gVMhHKqSSRYJyPsuoPNFH", null)}
    ]
  }},
  ""_links"": {{
    ""self"": {{
      ""href"": ""https://api.mollie.com/v2/payouts"",
      ""type"": ""application/hal+json""
    }},
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/list-payouts"",
      ""type"": ""text/html""
    }}
  }}
}}";
    }

    private string CreatePayoutJsonResponse(
        string payoutId,
        string balanceId,
        Amount? amount,
        string? description = null) {
        string amountJson = amount != null
            ? $@"{{""currency"": ""{amount.Currency}"", ""value"": ""{amount.Value}""}}"
            : @"{""currency"": ""EUR"", ""value"": ""10.00""}";
        string descriptionJson = description != null ? $@"""{description}""" : @"""My payout description""";

        return $@"{{
  ""resource"": ""payout"",
  ""id"": ""{payoutId}"",
  ""balanceId"": ""{balanceId}"",
  ""amount"": {amountJson},
  ""description"": {descriptionJson},
  ""status"": ""requested"",
  ""statusReason"": {{
    ""code"": ""requested"",
    ""message"": ""The payout has been requested.""
  }},
  ""createdAt"": ""2024-03-20T09:13:37+00:00"",
  ""initiatedAt"": null,
  ""completedAt"": null,
  ""canceledAt"": null,
  ""mode"": ""live"",
  ""_links"": {{
    ""self"": {{
      ""href"": ""https://api.mollie.com/v2/payouts/{payoutId}"",
      ""type"": ""application/hal+json""
    }},
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/get-payout"",
      ""type"": ""text/html""
    }}
  }}
}}";
    }
}

