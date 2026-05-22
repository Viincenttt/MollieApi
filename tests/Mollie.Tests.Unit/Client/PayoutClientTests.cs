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

