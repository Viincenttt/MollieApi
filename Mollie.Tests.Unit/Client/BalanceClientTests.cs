﻿using System;
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

      [Test]
      public async Task ListBalancesAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request a list of balances
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalancesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balances = await balanceClient.ListBalancesAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balances);
          Assert.AreEqual(2, balances.Count);
          Assert.AreEqual(2, balances.Items.Count);
      }
      
      private readonly string DefaultListBalancesResponse = $@"{{
  ""count"": 2,
  ""_embedded"": {{
    ""balances"": [
       {{
         ""resource"": ""balance"",
         ""id"": ""bal_gVMhHKqSSRYJyPsuoPNFH"",
         ""mode"": ""live"",
         ""createdAt"": ""2019-01-10T12:06:28+00:00"",
         ""currency"": ""EUR"",
         ""status"": ""active"",
         ""availableAmount"": {{
           ""value"": ""0.00"",
           ""currency"": ""EUR""
         }},
         ""pendingAmount"": {{
           ""value"": ""0.00"",
           ""currency"": ""EUR""
         }},
         ""transferFrequency"": ""daily"",
         ""transferThreshold"": {{
           ""value"": ""40.00"",
           ""currency"": ""EUR""
         }},
         ""transferReference"": ""Mollie payout"",
         ""transferDestination"": {{
           ""type"": ""bank-account"",
           ""beneficiaryName"": ""Jack Bauer"",
           ""bankAccount"": ""NL53INGB0654422370"",
           ""bankAccountId"": ""bnk_jrty3f""
         }},
         ""_links"": {{
           ""self"": {{
             ""href"": ""https://api.mollie.com/v2/balances/bal_gVMhHKqSSRYJyPsuoPNFH"",
             ""type"": ""application/hal+json""
           }}
         }}
       }},
       {{
         ""resource"": ""balance"",
         ""id"": ""bal_gVMhHKqSSRYJyPsuoPABC"",
         ""mode"": ""live"",
         ""createdAt"": ""2019-01-10T10:23:41+00:00"",
         ""status"": ""active"",
         ""currency"": ""EUR"",
         ""availableAmount"": {{
           ""value"": ""0.00"",
           ""currency"": ""EUR""
         }},
         ""pendingAmount"": {{
           ""value"": ""0.00"",
           ""currency"": ""EUR""
         }},
         ""transferFrequency"": ""twice-a-month"",
         ""transferThreshold"": {{
           ""value"": ""5.00"",
           ""currency"": ""EUR""
         }},
         ""transferReference"": ""Mollie payout"",
         ""transferDestination"": {{
           ""type"": ""bank-account"",
           ""beneficiaryName"": ""Jack Bauer"",
           ""bankAccount"": ""NL97MOLL6351480700"",
           ""bankAccountId"": ""bnk_jrty3e""
         }},
         ""_links"": {{
           ""self"": {{
             ""href"": ""https://api.mollie.com/v2/balances/bal_gVMhHKqSSRYJyPsuoPABC"",
             ""type"": ""application/hal+json""
           }}
         }}
       }}
    ]
  }},
  ""_links"": {{
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/v2/balances-api/list-balances"",
      ""type"": ""text/html""
    }},
    ""self"": {{
      ""href"": ""https://api.mollie.com/v2/balances?limit=5"",
      ""type"": ""application/hal+json""
    }},
    ""previous"": null,
    ""next"": {{
      ""href"": ""https://api.mollie.com/v2/balances?from=bal_gVMhHKqSSRYJyPsuoPABC&limit=5"",
      ""type"": ""application/hal+json""
    }}
  }}
}}";

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