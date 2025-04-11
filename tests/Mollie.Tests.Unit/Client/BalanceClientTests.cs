using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class BalanceClientTests : BaseClientTests {
      [Fact]
      public async Task GetBalanceAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request a specific balance
          var getBalanceResponseFactory = new GetBalanceResponseFactory();
          var getBalanceResponse = getBalanceResponseFactory.CreateGetBalanceResponse();
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{getBalanceResponseFactory.BalanceId}";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, getBalanceResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          BalanceResponse balanceResponse = await balanceClient.GetBalanceAsync(getBalanceResponseFactory.BalanceId);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceResponse.ShouldNotBeNull();
          balanceResponse.Id.ShouldBe(getBalanceResponseFactory.BalanceId);
          balanceResponse.CreatedAt.ToUniversalTime().ShouldBe(getBalanceResponseFactory.CreatedAt);
          balanceResponse.TransferThreshold.Currency.ShouldBe(getBalanceResponseFactory.TransferThreshold.Currency);
          balanceResponse.Currency.ShouldBe(getBalanceResponseFactory.Currency);
          balanceResponse.Status.ShouldBe(getBalanceResponseFactory.Status);
          balanceResponse.AvailableAmount.Currency.ShouldBe(getBalanceResponseFactory.AvailableAmount.Currency);
          balanceResponse.AvailableAmount.Value.ShouldBe(getBalanceResponseFactory.AvailableAmount.Value);
          balanceResponse.PendingAmount.Value.ShouldBe(getBalanceResponseFactory.PendingAmount.Value);
          balanceResponse.PendingAmount.Currency.ShouldBe(getBalanceResponseFactory.PendingAmount.Currency);
          balanceResponse.TransferFrequency.ShouldBe(getBalanceResponseFactory.TransferFrequency);
          balanceResponse.TransferReference.ShouldBe(getBalanceResponseFactory.TransferReference);
          balanceResponse.TransferThreshold.Currency.ShouldBe(getBalanceResponseFactory.TransferThreshold.Currency);
          balanceResponse.TransferThreshold.Value.ShouldBe(getBalanceResponseFactory.TransferThreshold.Value);
          balanceResponse.TransferDestination.Type.ShouldBe(getBalanceResponseFactory.TransferDestination.Type);
          balanceResponse.TransferDestination.BankAccount.ShouldBe(getBalanceResponseFactory.TransferDestination.BankAccount);
          balanceResponse.TransferDestination.BeneficiaryName.ShouldBe(getBalanceResponseFactory.TransferDestination.BeneficiaryName);
          balanceResponse.Links.ShouldNotBeNull();
          balanceResponse.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/balances/{getBalanceResponseFactory.BalanceId}");
          balanceResponse.Links.Self.Type.ShouldBe("application/hal+json");
          balanceResponse.Links.Documentation.Href.ShouldBe($"https://docs.mollie.com/reference/v2/balances-api/get-balance");
          balanceResponse.Links.Documentation.Type.ShouldBe("text/html");
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public async Task GetBalanceAsync_NoBalanceIdIsGiven_ArgumentExceptionIsThrown(string? balanceId) {
          // Arrange
          var mockHttp = new MockHttpMessageHandler();
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
          var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await balanceClient.GetBalanceAsync(balanceId));
#pragma warning restore CS8604 // Possible null reference argument.

          // Then
          exception.Message.ShouldBe("Required URL argument 'balanceId' is null or empty");
      }

      [Fact]
      public async Task GetPrimaryBalanceAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request the primary balance
          var getBalanceResponseFactory = new GetBalanceResponseFactory();
          var getBalanceResponse = getBalanceResponseFactory.CreateGetBalanceResponse();
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/primary";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, getBalanceResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          BalanceResponse balanceResponse = await balanceClient.GetPrimaryBalanceAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceResponse.ShouldNotBeNull();
          balanceResponse.ShouldNotBeNull();
          balanceResponse.Id.ShouldBe(getBalanceResponseFactory.BalanceId);
          balanceResponse.CreatedAt.ToUniversalTime().ShouldBe(getBalanceResponseFactory.CreatedAt);
          balanceResponse.TransferThreshold.Currency.ShouldBe(getBalanceResponseFactory.TransferThreshold.Currency);
          balanceResponse.Currency.ShouldBe(getBalanceResponseFactory.Currency);
          balanceResponse.Status.ShouldBe(getBalanceResponseFactory.Status);
          balanceResponse.AvailableAmount.Currency.ShouldBe(getBalanceResponseFactory.AvailableAmount.Currency);
          balanceResponse.AvailableAmount.Value.ShouldBe(getBalanceResponseFactory.AvailableAmount.Value);
          balanceResponse.PendingAmount.Value.ShouldBe(getBalanceResponseFactory.PendingAmount.Value);
          balanceResponse.PendingAmount.Currency.ShouldBe(getBalanceResponseFactory.PendingAmount.Currency);
          balanceResponse.TransferFrequency.ShouldBe(getBalanceResponseFactory.TransferFrequency);
          balanceResponse.TransferReference.ShouldBe(getBalanceResponseFactory.TransferReference);
          balanceResponse.TransferThreshold.Currency.ShouldBe(getBalanceResponseFactory.TransferThreshold.Currency);
          balanceResponse.TransferThreshold.Value.ShouldBe(getBalanceResponseFactory.TransferThreshold.Value);
          balanceResponse.TransferDestination.Type.ShouldBe(getBalanceResponseFactory.TransferDestination.Type);
          balanceResponse.TransferDestination.BankAccount.ShouldBe(getBalanceResponseFactory.TransferDestination.BankAccount);
          balanceResponse.TransferDestination.BeneficiaryName.ShouldBe(getBalanceResponseFactory.TransferDestination.BeneficiaryName);
      }

      [Fact]
      public async Task ListBalancesAsync_DefaultBehaviour_ResponseIsParsed() {
          // Given: We request a list of balances
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalancesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balances = await balanceClient.GetBalanceListAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balances.ShouldNotBeNull();
          balances.Count.ShouldBe(2);
          balances.Items.Count.ShouldBe(2);
      }

      [Fact]
      public async Task GetBalanceReportAsync_TransactionCategories_ResponseIsParsed() {
          // Given: We request a balance report
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          DateTime from = new DateTime(2022, 11, 1);
          DateTime until = new DateTime(2022, 11, 30);
          string grouping = ReportGrouping.TransactionCategories;

          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/report" +
                               $"?from={from.ToString("yyyy-MM-dd")}&until={until.ToString("yyyy-MM-dd")}&grouping={grouping}";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultGetBalanceReportTransactionCategoriesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceReport = await balanceClient.GetBalanceReportAsync(balanceId, from, until, grouping);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceReport.ShouldNotBeNull();
          balanceReport.ShouldBeOfType<TransactionCategoriesReportResponse>();
          var specificBalanceReport = (TransactionCategoriesReportResponse)balanceReport;
          specificBalanceReport.Grouping.ShouldBe(grouping);
          specificBalanceReport.BalanceId.ShouldBe(balanceId);
          specificBalanceReport.Resource.ShouldBe("balance-report");
          specificBalanceReport.From.ShouldBe(from);
          specificBalanceReport.Until.ShouldBe(until);
          specificBalanceReport.Totals.ShouldNotBeNull();
          specificBalanceReport.Totals.Open.Pending.Amount.Value.ShouldBe("5.30");
          specificBalanceReport.Totals.Open.Pending.Amount.Currency.ShouldBe("EUR");
          specificBalanceReport.Totals.Open.Available.Amount.Value.ShouldBe("0.11");
          specificBalanceReport.Totals.Open.Available.Amount.Currency.ShouldBe("EUR");
          var childSubTotals = specificBalanceReport.Totals.Payments.Pending.Subtotals.First();
          childSubTotals.TransactionType.ShouldBe("payment");
          childSubTotals.Count.ShouldBe(36);
          var childChildSubTotals = childSubTotals.Subtotals!.First();
          childChildSubTotals.Method.ShouldBe("ideal");
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public async Task GetBalanceReportAsync_NoBalanceIdIsGiven_ArgumentExceptionIsThrown(string? balanceId) {
          // Arrange
          var mockHttp = new MockHttpMessageHandler();
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);
          DateTime from = new DateTime(2022, 11, 1);
          DateTime until = new DateTime(2022, 11, 30);

          // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
          var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await balanceClient.GetBalanceReportAsync(balanceId, from, until));
#pragma warning restore CS8604 // Possible null reference argument.

          // Then
          exception.Message.ShouldBe("Required URL argument 'balanceId' is null or empty");
      }

      [Fact]
      public async Task GetBalanceReportAsync_StatusBalances_ResponseIsParsed() {
          // Given: We request a balance report
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          DateTime from = new DateTime(2022, 11, 1);
          DateTime until = new DateTime(2022, 11, 30);
          string grouping = ReportGrouping.StatusBalances;

          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/report" +
                               $"?from={from.ToString("yyyy-MM-dd")}&until={until.ToString("yyyy-MM-dd")}&grouping={grouping}";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultGetBalanceReportStatusBalancesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceReport = await balanceClient.GetBalanceReportAsync(balanceId, from, until, grouping);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceReport.ShouldNotBeNull();
          balanceReport.ShouldBeOfType<StatusBalanceReportResponse>();
          var specificBalanceReport = (StatusBalanceReportResponse)balanceReport;
          specificBalanceReport.Grouping.ShouldBe(grouping);
          specificBalanceReport.BalanceId.ShouldBe(balanceId);
          specificBalanceReport.Resource.ShouldBe("balance-report");
          specificBalanceReport.From.ShouldBe(from);
          specificBalanceReport.Until.ShouldBe(until);
          specificBalanceReport.Totals.ShouldNotBeNull();
          specificBalanceReport.Totals.PendingBalance.Open.Amount.Value.ShouldBe("5.30");
          specificBalanceReport.Totals.PendingBalance.Open.Amount.Currency.ShouldBe(Currency.EUR);
          specificBalanceReport.Totals.AvailableBalance.MovedFromPending.Amount.Value.ShouldBe("3.38");
          specificBalanceReport.Totals.AvailableBalance.MovedFromPending.Amount.Currency.ShouldBe(Currency.EUR);
          var childSubTotals = specificBalanceReport.Totals.AvailableBalance.MovedFromPending.Subtotals.First();
          childSubTotals.TransactionType.ShouldBe("payment");
          var childChildSubtotals = childSubTotals.Subtotals!.First();
          childChildSubtotals.Method.ShouldBe("ideal");
      }

      [Fact]
      public async Task ListBalanceTransactionsAsync_StatusBalances_ResponseIsParsed() {
          // Given
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/transactions";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalanceTransactionsResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceTransactions = await balanceClient.GetBalanceTransactionListAsync(balanceId);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceTransactions.Count.ShouldBe(balanceTransactions.Items.Count);
          var transaction = balanceTransactions.Items.First();
          transaction.Resource.ShouldBe("balance_transactions");
          transaction.Id.ShouldBe("baltr_9S8yk4FFqqi2Qm6K3rqRH");
          transaction.Type.ShouldBe("outgoing-transfer");
          transaction.ResultAmount.Value.ShouldBe("-7.76");
          transaction.ResultAmount.Currency.ShouldBe(Currency.EUR);
          transaction.InitialAmount.Value.ShouldBe("-7.76");
          transaction.InitialAmount.Currency.ShouldBe(Currency.EUR);
          transaction.ShouldBeOfType<SettlementBalanceTransactionResponse>();
          var transactionContext = (SettlementBalanceTransactionResponse)transaction;
          transactionContext.Context.SettlementId.ShouldBe("stl_ma2vu8");
          transactionContext.Context.TransferId.ShouldBe("trf_ma2vu8");
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public async Task ListBalanceTransactionsAsync_NoBalanceIdIsGiven_ArgumentExceptionIsThrown(string? balanceId) {
          // Arrange
          var mockHttp = new MockHttpMessageHandler();
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
          var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await balanceClient.GetBalanceTransactionListAsync(balanceId));
#pragma warning restore CS8604 // Possible null reference argument.

          // Then
          exception.Message.ShouldBe("Required URL argument 'balanceId' is null or empty");
      }

      [Fact]
      public async Task ListPrimaryBalanceTransactionsAsync_StatusBalances_ResponseIsParsed() {
          // Given
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/primary/transactions";
          var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalanceTransactionsResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceTransactions = await balanceClient.GetPrimaryBalanceTransactionListAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          balanceTransactions.Count.ShouldBe(balanceTransactions.Items.Count);
      }

      private readonly string DefaultListBalanceTransactionsResponse = @"
{
    ""_embedded"": {
        ""balance_transactions"": [{
                ""resource"": ""balance_transactions"",
                ""id"": ""baltr_9S8yk4FFqqi2Qm6K3rqRH"",
                ""type"": ""outgoing-transfer"",
                ""resultAmount"": {
                    ""value"": ""-7.76"",
                    ""currency"": ""EUR""
                },
                ""initialAmount"": {
                    ""value"": ""-7.76"",
                    ""currency"": ""EUR""
                },
                ""createdAt"": ""2022-12-21T05:02:50+00:00"",
                ""context"": {
                    ""settlementId"": ""stl_ma2vu8"",
                    ""transferId"": ""trf_ma2vu8""
                }
            }, {
                ""resource"": ""balance_transactions"",
                ""id"": ""baltr_2cXFV9Wd9AjYqHa8i2qRH"",
                ""type"": ""payment"",
                ""resultAmount"": {
                    ""value"": ""0.15"",
                    ""currency"": ""EUR""
                },
                ""initialAmount"": {
                    ""value"": ""0.15"",
                    ""currency"": ""EUR""
                },
                ""createdAt"": ""2022-12-20T21:21:09+00:00"",
                ""context"": {
                    ""paymentId"": ""tr_JzT2KxV7hZ""
                }
            }
        ]
    },
    ""count"": 2,
    ""_links"": {
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/balances-api/list-balance-transactions"",
            ""type"": ""text/html""
        },
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/balances/bal_CKjKwQdjCwCSArXFAJNFH/transactions?limit=50"",
            ""type"": ""application/hal+json""
        },
        ""previous"": null,
        ""next"": {
            ""href"": ""https://api.mollie.com/v2/balances/bal_CKjKwQdjCwCSArXFAJNFH/transactions?from=baltr_pEM6remSK3UGHDRAuzVRH\u0026limit=50"",
            ""type"": ""application/hal+json""
        }
    }
}";

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

      private readonly string DefaultGetBalanceReportTransactionCategoriesResponse = @"{
    ""resource"": ""balance-report"",
    ""balanceId"": ""bal_CKjKwQdjCwCSArXFAJNFH"",
    ""timeZone"": ""Europe/Amsterdam"",
    ""from"": ""2022-11-01"",
    ""until"": ""2022-11-30"",
    ""grouping"": ""transaction-categories"",
    ""totals"": {
        ""open"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""5.30"",
                    ""currency"": ""EUR""
                }
            },
            ""available"": {
                ""amount"": {
                    ""value"": ""0.11"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""payments"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""3.57"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""payment"",
                        ""count"": 36,
                        ""amount"": {
                            ""value"": ""1.07"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""0.02"",
                                    ""currency"": ""EUR""
                                }
                            }, {
                                ""method"": ""pointofsale"",
                                ""count"": 35,
                                ""amount"": {
                                    ""value"": ""1.05"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""split-payment"",
                        ""count"": 3,
                        ""amount"": {
                            ""value"": ""2.50"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 3,
                                ""amount"": {
                                    ""value"": ""2.50"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }
                ]
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""3.66"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""payment"",
                        ""count"": 36,
                        ""amount"": {
                            ""value"": ""1.07"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""0.02"",
                                    ""currency"": ""EUR""
                                }
                            }, {
                                ""method"": ""pointofsale"",
                                ""count"": 35,
                                ""amount"": {
                                    ""value"": ""1.05"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""split-payment"",
                        ""count"": 3,
                        ""amount"": {
                            ""value"": ""2.50"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 3,
                                ""amount"": {
                                    ""value"": ""2.50"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""released-rolling-reserve"",
                        ""amount"": {
                            ""value"": ""0.09"",
                            ""currency"": ""EUR""
                        }
                    }
                ]
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""refunds"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""-1.00"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""refund"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""-1.00"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""creditcard"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-1.00"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""cardIssuer"": ""other"",
                                        ""cardAudience"": ""other"",
                                        ""cardRegion"": ""domestic"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-1.00"",
                                            ""currency"": ""EUR""
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        },
        ""chargebacks"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""capital"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""transfers"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""fee-prepayments"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""-0.28"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""prepaymentPartType"": ""fee"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""-0.28"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""feeType"": ""payment-fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.28"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""method"": ""ideal"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.28"",
                                            ""currency"": ""EUR""
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""-0.28"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""prepaymentPartType"": ""fee"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""-0.28"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""feeType"": ""payment-fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.28"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""method"": ""ideal"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.28"",
                                            ""currency"": ""EUR""
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""-0.24"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""prepaymentPartType"": ""fee"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""-0.25"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""feeType"": ""refund-fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.25"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""invoice-rounding-compensation"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""0.01"",
                            ""currency"": ""EUR""
                        }
                    }
                ]
            }
        },
        ""corrections"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""0.00"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""close"": {
            ""pending"": {
                ""amount"": {
                    ""value"": ""5.21"",
                    ""currency"": ""EUR""
                }
            },
            ""available"": {
                ""amount"": {
                    ""value"": ""2.25"",
                    ""currency"": ""EUR""
                }
            }
        }
    },
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/balances/bal_CKjKwQdjCwCSArXFAJNFH/report"",
            ""type"": ""application/hal+json""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/balances-api/get-balance-report"",
            ""type"": ""text/html""
        }
    }
}";

      private readonly string DefaultGetBalanceReportStatusBalancesResponse = @"{
    ""resource"": ""balance-report"",
    ""balanceId"": ""bal_CKjKwQdjCwCSArXFAJNFH"",
    ""timeZone"": ""Europe/Amsterdam"",
    ""from"": ""2022-11-01"",
    ""until"": ""2022-11-30"",
    ""grouping"": ""status-balances"",
    ""totals"": {
        ""pendingBalance"": {
            ""open"": {
                ""amount"": {
                    ""value"": ""5.30"",
                    ""currency"": ""EUR""
                }
            },
            ""pending"": {
                ""amount"": {
                    ""value"": ""3.29"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""payment"",
                        ""count"": 36,
                        ""amount"": {
                            ""value"": ""1.07"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""0.02"",
                                    ""currency"": ""EUR""
                                }
                            }, {
                                ""method"": ""pointofsale"",
                                ""count"": 35,
                                ""amount"": {
                                    ""value"": ""1.05"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""split-payment"",
                        ""count"": 3,
                        ""amount"": {
                            ""value"": ""2.50"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 3,
                                ""amount"": {
                                    ""value"": ""2.50"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""fee-prepayment"",
                        ""amount"": {
                            ""value"": ""-0.28"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""prepaymentPartType"": ""fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.28"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""feeType"": ""payment-fee"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.28"",
                                            ""currency"": ""EUR""
                                        },
                                        ""subtotals"": [{
                                                ""method"": ""ideal"",
                                                ""count"": 1,
                                                ""amount"": {
                                                    ""value"": ""-0.28"",
                                                    ""currency"": ""EUR""
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            ""movedToAvailable"": {
                ""amount"": {
                    ""value"": ""3.38"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""payment"",
                        ""count"": 36,
                        ""amount"": {
                            ""value"": ""1.07"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""0.02"",
                                    ""currency"": ""EUR""
                                }
                            }, {
                                ""method"": ""pointofsale"",
                                ""count"": 35,
                                ""amount"": {
                                    ""value"": ""1.05"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""split-payment"",
                        ""count"": 3,
                        ""amount"": {
                            ""value"": ""2.50"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 3,
                                ""amount"": {
                                    ""value"": ""2.50"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""fee-prepayment"",
                        ""amount"": {
                            ""value"": ""-0.28"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""prepaymentPartType"": ""fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.28"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""feeType"": ""payment-fee"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.28"",
                                            ""currency"": ""EUR""
                                        },
                                        ""subtotals"": [{
                                                ""method"": ""ideal"",
                                                ""count"": 1,
                                                ""amount"": {
                                                    ""value"": ""-0.28"",
                                                    ""currency"": ""EUR""
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }, {
                        ""transactionType"": ""released-rolling-reserve"",
                        ""amount"": {
                            ""value"": ""0.09"",
                            ""currency"": ""EUR""
                        }
                    }
                ]
            },
            ""close"": {
                ""amount"": {
                    ""value"": ""5.21"",
                    ""currency"": ""EUR""
                }
            }
        },
        ""availableBalance"": {
            ""open"": {
                ""amount"": {
                    ""value"": ""0.11"",
                    ""currency"": ""EUR""
                }
            },
            ""movedFromPending"": {
                ""amount"": {
                    ""value"": ""3.38"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""payment"",
                        ""count"": 36,
                        ""amount"": {
                            ""value"": ""1.07"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""0.02"",
                                    ""currency"": ""EUR""
                                }
                            }, {
                                ""method"": ""pointofsale"",
                                ""count"": 35,
                                ""amount"": {
                                    ""value"": ""1.05"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""split-payment"",
                        ""count"": 3,
                        ""amount"": {
                            ""value"": ""2.50"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""ideal"",
                                ""count"": 3,
                                ""amount"": {
                                    ""value"": ""2.50"",
                                    ""currency"": ""EUR""
                                }
                            }
                        ]
                    }, {
                        ""transactionType"": ""fee-prepayment"",
                        ""amount"": {
                            ""value"": ""-0.28"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""prepaymentPartType"": ""fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.28"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""feeType"": ""payment-fee"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.28"",
                                            ""currency"": ""EUR""
                                        },
                                        ""subtotals"": [{
                                                ""method"": ""ideal"",
                                                ""count"": 1,
                                                ""amount"": {
                                                    ""value"": ""-0.28"",
                                                    ""currency"": ""EUR""
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }, {
                        ""transactionType"": ""released-rolling-reserve"",
                        ""amount"": {
                            ""value"": ""0.09"",
                            ""currency"": ""EUR""
                        }
                    }
                ]
            },
            ""immediatelyAvailable"": {
                ""amount"": {
                    ""value"": ""-1.24"",
                    ""currency"": ""EUR""
                },
                ""subtotals"": [{
                        ""transactionType"": ""refund"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""-1.00"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""method"": ""creditcard"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-1.00"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""cardIssuer"": ""other"",
                                        ""cardAudience"": ""other"",
                                        ""cardRegion"": ""domestic"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-1.00"",
                                            ""currency"": ""EUR""
                                        }
                                    }
                                ]
                            }
                        ]
                    }, {
                        ""transactionType"": ""fee-prepayment"",
                        ""amount"": {
                            ""value"": ""-0.25"",
                            ""currency"": ""EUR""
                        },
                        ""subtotals"": [{
                                ""prepaymentPartType"": ""fee"",
                                ""count"": 1,
                                ""amount"": {
                                    ""value"": ""-0.25"",
                                    ""currency"": ""EUR""
                                },
                                ""subtotals"": [{
                                        ""feeType"": ""refund-fee"",
                                        ""count"": 1,
                                        ""amount"": {
                                            ""value"": ""-0.25"",
                                            ""currency"": ""EUR""
                                        }
                                    }
                                ]
                            }
                        ]
                    }, {
                        ""transactionType"": ""invoice-rounding-compensation"",
                        ""count"": 1,
                        ""amount"": {
                            ""value"": ""0.01"",
                            ""currency"": ""EUR""
                        }
                    }
                ]
            },
            ""close"": {
                ""amount"": {
                    ""value"": ""2.25"",
                    ""currency"": ""EUR""
                }
            }
        }
    },
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/balances/bal_CKjKwQdjCwCSArXFAJNFH/report"",
            ""type"": ""application/hal+json""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/balances-api/get-balance-report"",
            ""type"": ""text/html""
        }
    }
}";

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
