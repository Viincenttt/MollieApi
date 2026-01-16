using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.BalanceTransfer;
using Mollie.Api.Models.BalanceTransfer.Request;
using Mollie.Api.Models.BalanceTransfer.Response;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;
using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Unit.Client;

public class BalanceTransferClientTests : BaseClientTests {
    [Fact]
    public async Task CreateBalanceTransferAsync_WithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
        // Given: we create a balance transfer request with only the required parameters
        const string balanceTransferId = "balance-transfer-id";
        BalanceTransferRequest request = new() {
            Description = "Test Description",
            Amount = new Amount(Currency.EUR, 50),
            Source = new BalanceTransferParty {
                Id = "source",
                Description = "Test Source",
                Type = "organization"
            },
            Destination = new BalanceTransferParty {
                Id = "destination",
                Description = "Test Destination",
                Type = "organization"
            }
        };
        string jsonToReturnInMockResponse = CreateBalanceTransferJsonResponse(balanceTransferId, request);
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}connect/balance-transfers", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new BalanceTransferClient("abcde", httpClient);

        // When: We send the request
        BalanceTransferResponse response = await client.CreateBalanceTransferAsync(request);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Description.ShouldBe(request.Description);
        response.Amount.ShouldBeEquivalentTo(request.Amount);
        response.Source.ShouldBeEquivalentTo(response.Source);
        response.Destination.ShouldBeEquivalentTo(response.Destination);
        response.Status.ShouldBe("succeeded");
        response.StatusReason.Code.ShouldBe("success");
        response.StatusReason.Message.ShouldBe("Balance transfer completed successfully.");
        response.CreatedAt.ToUniversalTime().ShouldBe(new DateTime(2025, 5, 1, 10, 0, 0, DateTimeKind.Utc));
        response.ExecutedAt!.Value.ToUniversalTime().ShouldBe(new DateTime(2025, 5, 1, 10, 5, 0, DateTimeKind.Utc));
        response.Mode.ShouldBe(Mode.Live);
    }

    [Theory]
    [InlineData(null, null,  false, null, "")]
    [InlineData("from", null,  false, null, "?from=from")]
    [InlineData("from", 50,  false, null, "?from=from&limit=50")]
    [InlineData(null, null,  true, null, "?testmode=true")]
    [InlineData(null, null,  true, SortDirection.Desc, "?testmode=true&sort=desc")]
    [InlineData(null, null,  true, SortDirection.Asc, "?testmode=true&sort=asc")]
    public async Task GetBalanceTransferListAsync_TestModeParameterCase_QueryStringOnlyContainsTestModeParameterIfTrue( string? from,
        int? limit,
        bool testmode,
        SortDirection? sortDirection,
        string expectedQueryString) {
        // Given: We retrieve a list of customers
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}connect/balance-transfers{expectedQueryString}")
            .Respond("application/json", CreateBalanceTransferListJsonResponse());
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new BalanceTransferClient("abcde", httpClient);

        // When: We send the request
        var result = await client.GetBalanceTransferListAsync(from, limit, sortDirection, testmode);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBalanceTransferAsync_WithValidBalanceTransferId_ResponseIsDeserializedInExpectedFormat() {
        // Given: We have a valid balance transfer ID
        const string balanceTransferId = "balance-transfer-id";
        BalanceTransferRequest request = new() {
            Description = "Test Description",
            Amount = new Amount(Currency.EUR, 50),
            Source = new BalanceTransferParty {
                Id = "source",
                Description = "Test Source",
                Type = "organization"
            },
            Destination = new BalanceTransferParty {
                Id = "destination",
                Description = "Test Destination",
                Type = "organization"
            }
        };
        string jsonToReturnInMockResponse = CreateBalanceTransferJsonResponse(balanceTransferId, request);
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}connect/balance-transfers/{balanceTransferId}", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new BalanceTransferClient("abcde", httpClient);

        // When: We attempt to retrieve the balance transfer
        BalanceTransferResponse response = await client.GetBalanceTransferAsync(balanceTransferId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Id.ShouldBe(balanceTransferId);
        response.Description.ShouldBe(request.Description);
    }

    [Fact]
    public async Task GetBalanceTransferAsync_WithTestmodeTrue_QueryStringContainsTestmodeParameter() {
        // Given: We have a valid balance transfer ID
        const string balanceTransferId = "balance-transfer-id";
        BalanceTransferRequest request = new() {
            Description = "Test Description",
            Amount = new Amount(Currency.EUR, 50),
            Source = new BalanceTransferParty {
                Id = "source",
                Description = "Test Source",
                Type = "organization"
            },
            Destination = new BalanceTransferParty {
                Id = "destination",
                Description = "Test Destination",
                Type = "organization"
            }
        };
        string jsonToReturnInMockResponse = CreateBalanceTransferJsonResponse(balanceTransferId, request);
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}connect/balance-transfers/{balanceTransferId}?testmode=true", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new BalanceTransferClient("abcde", httpClient);

        // When: We attempt to retrieve the balance transfer
        BalanceTransferResponse response = await client.GetBalanceTransferAsync(balanceTransferId, testmode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.ShouldNotBeNull();
        response.Id.ShouldBe(balanceTransferId);
        response.Description.ShouldBe(request.Description);
    }

    [Fact]
    public async Task GetBalanceTransferAsync_WithEmptyBalanceTransferId_ThrowsException() {
        // Given: We have an empty balance transfer ID
        var client = new BalanceTransferClient("abcde", new HttpClient());

        // When: We attempt to retrieve the balance transfer
        ArgumentException exception = await Should.ThrowAsync<ArgumentException>(async () => {
            await client.GetBalanceTransferAsync("");
        });

        // Then
        exception.Message.ShouldContain("balanceTransferId");
    }


    private string CreateBalanceTransferListJsonResponse() {
        BalanceTransferRequest request = new() {
            Description = "Test Description",
            Amount = new Amount(Currency.EUR, 50),
            Source = new BalanceTransferParty {
                Id = "source",
                Description = "Test Source",
                Type = "organization"
            },
            Destination = new BalanceTransferParty {
                Id = "destination",
                Description = "Test Destination",
                Type = "organization"
            }
        };

        return $@"{{
          ""count"": 1,
          ""_embedded"": {{
            ""connect-balance-transfers"": [
                {CreateBalanceTransferJsonResponse("balance-transfer-id-1", request)},
                {CreateBalanceTransferJsonResponse("balance-transfer-id-2", request)},
            ]
          }}
        }}";
    }


    private string CreateBalanceTransferJsonResponse(string balanceTransferId, BalanceTransferRequest request) {
        return $@"{{
  ""resource"": ""connect-balance-transfer"",
  ""id"": ""{balanceTransferId}"",
  ""amount"": {{
    ""value"": ""{request.Amount.Value}"",
    ""currency"": ""{request.Amount.Currency}""
  }},
  ""source"": {{
    ""type"": ""{request.Source.Type}"",
    ""id"": ""{request.Source.Id}"",
    ""description"": ""{request.Source.Description}""
  }},
  ""destination"": {{
    ""type"": ""{request.Destination.Type}"",
    ""id"": ""{request.Destination.Id}"",
    ""description"": ""{request.Destination.Description}""
  }},
  ""description"": ""{request.Description}"",
  ""status"": ""succeeded"",
  ""statusReason"": {{
    ""code"": ""success"",
    ""message"": ""Balance transfer completed successfully.""
  }},
  ""metadata"": {{
    ""order_id"": 12345,
    ""customer_id"": 9876
  }},
  ""createdAt"": ""2025-05-01T10:00:00+00:00"",
  ""executedAt"": ""2025-05-01T10:05:00+00:00"",
  ""mode"": ""live""
}}";
    }
}
