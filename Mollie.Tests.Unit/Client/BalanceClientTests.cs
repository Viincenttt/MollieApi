using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Balance.Response;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Client {
    public class BalanceClientTests : BaseClientTests {
      [Test]
      public async Task GetBalanceAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request a specific balance
          var getBalanceResponseFactory = new GetBalanceResponseFactory();
          var getBalanceResponse = getBalanceResponseFactory.CreateGetBalanceResponse();
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{getBalanceResponseFactory.BalanceId}";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, getBalanceResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          BalanceResponse balanceResponse = await balanceClient.GetBalanceAsync(getBalanceResponseFactory.BalanceId);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceResponse);
          Assert.AreEqual(getBalanceResponseFactory.BalanceId, balanceResponse.Id);
          Assert.AreEqual(getBalanceResponseFactory.CreatedAt, balanceResponse.CreatedAt.ToUniversalTime());
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Currency, balanceResponse.TransferThreshold.Currency);
          Assert.AreEqual(getBalanceResponseFactory.Currency, balanceResponse.Currency);
          Assert.AreEqual(getBalanceResponseFactory.Status, balanceResponse.Status);
          Assert.AreEqual(getBalanceResponseFactory.AvailableAmount.Currency, balanceResponse.AvailableAmount.Currency);
          Assert.AreEqual(getBalanceResponseFactory.AvailableAmount.Value, balanceResponse.AvailableAmount.Value);
          Assert.AreEqual(getBalanceResponseFactory.PendingAmount.Currency, balanceResponse.PendingAmount.Currency);
          Assert.AreEqual(getBalanceResponseFactory.PendingAmount.Value, balanceResponse.PendingAmount.Value);
          Assert.AreEqual(getBalanceResponseFactory.TransferFrequency, balanceResponse.TransferFrequency);
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Currency, balanceResponse.TransferThreshold.Currency);
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Value, balanceResponse.TransferThreshold.Value);
          Assert.AreEqual(getBalanceResponseFactory.TransferReference, balanceResponse.TransferReference);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.Type, balanceResponse.TransferDestination.Type);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.BankAccount, balanceResponse.TransferDestination.BankAccount);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.BeneficiaryName, balanceResponse.TransferDestination.BeneficiaryName);
      }

      [Test]
      public async Task GetPrimaryBalanceAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request the primary balance
          var getBalanceResponseFactory = new GetBalanceResponseFactory();
          var getBalanceResponse = getBalanceResponseFactory.CreateGetBalanceResponse();
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/primary";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, getBalanceResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          BalanceResponse balanceResponse = await balanceClient.GetPrimaryBalanceAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceResponse);
          Assert.AreEqual(getBalanceResponseFactory.BalanceId, balanceResponse.Id);
          Assert.AreEqual(getBalanceResponseFactory.CreatedAt, balanceResponse.CreatedAt.ToUniversalTime());
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Currency, balanceResponse.TransferThreshold.Currency);
          Assert.AreEqual(getBalanceResponseFactory.Currency, balanceResponse.Currency);
          Assert.AreEqual(getBalanceResponseFactory.Status, balanceResponse.Status);
          Assert.AreEqual(getBalanceResponseFactory.AvailableAmount.Currency, balanceResponse.AvailableAmount.Currency);
          Assert.AreEqual(getBalanceResponseFactory.AvailableAmount.Value, balanceResponse.AvailableAmount.Value);
          Assert.AreEqual(getBalanceResponseFactory.PendingAmount.Currency, balanceResponse.PendingAmount.Currency);
          Assert.AreEqual(getBalanceResponseFactory.PendingAmount.Value, balanceResponse.PendingAmount.Value);
          Assert.AreEqual(getBalanceResponseFactory.TransferFrequency, balanceResponse.TransferFrequency);
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Currency, balanceResponse.TransferThreshold.Currency);
          Assert.AreEqual(getBalanceResponseFactory.TransferThreshold.Value, balanceResponse.TransferThreshold.Value);
          Assert.AreEqual(getBalanceResponseFactory.TransferReference, balanceResponse.TransferReference);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.Type, balanceResponse.TransferDestination.Type);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.BankAccount, balanceResponse.TransferDestination.BankAccount);
          Assert.AreEqual(getBalanceResponseFactory.TransferDestination.BeneficiaryName, balanceResponse.TransferDestination.BeneficiaryName);
      }

      private class GetBalanceResponseFactory {
          public string BalanceId { get; set; } = "bal_gVMhHKqSSRYJyPsuoPNFH";
          public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(new DateTime(2022, 11, 22, 13, 15, 0), DateTimeKind.Utc);
          public string Currency { get; set; } = "EUR";
          public BalanceResponseStatus Status { get; set; } = BalanceResponseStatus.Active;
          public Amount AvailableAmount { get; set; } = new Amount(Api.Models.Currency.EUR, 905.25m);
          public Amount PendingAmount { get; set; } = new Amount(Api.Models.Currency.EUR, 100);
          public string TransferFrequency { get; set; } = "twice-a-month";
          public Amount TransferThreshold { get; set; } = new Amount(Api.Models.Currency.EUR, 5);
          public string TransferReference { get; set; } = "Mollie payout";

          public BalanceTransferDestination TransferDestination { get; set; } = new BalanceTransferDestination {
            Type = "bank-account",
            BankAccount = "bank-account",
            BeneficiaryName = "Piet"
          };

          public string CreateGetBalanceResponse() {
            return @$"
  {{
    ""resource"": ""balance"",
    ""id"": ""{BalanceId}"",
    ""mode"": ""live"",
    ""createdAt"": ""{CreatedAt:yyyy-MM-ddTHH:mm:ss.fffffffzzz}"",
    ""currency"": ""{Currency}"",
    ""status"": ""{Status}"",
    ""availableAmount"": {{
      ""value"": ""{AvailableAmount.Value}"",
      ""currency"": ""{AvailableAmount.Currency}""
    }},
    ""pendingAmount"": {{
      ""value"": ""{PendingAmount.Value}"",
      ""currency"": ""{PendingAmount.Currency}""
    }},
    ""transferFrequency"": ""{TransferFrequency}"",
    ""transferThreshold"": {{
      ""value"": ""{TransferThreshold.Value}"",
      ""currency"": ""{TransferThreshold.Currency}""
    }},
    ""transferReference"": ""{TransferReference}"",
    ""transferDestination"": {{
      ""type"": ""{TransferDestination.Type}"",
      ""beneficiaryName"": ""{TransferDestination.BeneficiaryName}"",
      ""bankAccount"": ""{TransferDestination.BankAccount}"",
      ""bankAccountId"": ""bnk_jrty3f""
    }},
    ""_links"": {{
      ""self"": {{
        ""href"": ""https://api.mollie.com/v2/balances/bal_gVMhHKqSSRYJyPsuoPNFH"",
        ""type"": ""application/hal+json""
      }},
      ""documentation"": {{
        ""href"": ""https://docs.mollie.com/reference/v2/balances-api/get-balance"",
        ""type"": ""text/html""
      }}
    }}
  }}";
        }
      }
    }
}