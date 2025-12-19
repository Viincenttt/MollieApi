using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.BalanceTransfer;
using Mollie.Api.Models.BalanceTransfer.Request;
using Mollie.Api.Models.BalanceTransfer.Response;
using Mollie.Api.Models.Webhook;
using Shouldly;
using Xunit;

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
        string jsonToReturnInMockResponse = CreateWebhookJsonResponse(balanceTransferId, request);
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
    }

    private string CreateWebhookJsonResponse(string balanceTransferId, BalanceTransferRequest request) {
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
