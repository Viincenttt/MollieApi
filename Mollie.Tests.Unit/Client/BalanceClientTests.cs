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
      
      [Test]
      public async Task GetBalanceReportAsync_TransactionCategories_ResponseIsParsed() {
          // Given: We request a balance report
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          DateTime from = new DateTime(2022, 11, 1);
          DateTime until = new DateTime(2022, 11, 30);
          string grouping = ReportGrouping.TransactionCategories;
          
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/report" +
                               $"?from={from.ToString("yyyy-MM-dd")}&until={until.ToString("yyyy-MM-dd")}&grouping={grouping}";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultGetBalanceReportTransactionCategoriesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceReport = await balanceClient.GetBalanceReportAsync(balanceId, from, until, grouping);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceReport);
          Assert.AreEqual(typeof(TransactionCategoriesReportResponse), balanceReport.GetType());
          var specificBalanceReport = (TransactionCategoriesReportResponse)balanceReport;
          Assert.AreEqual(grouping, specificBalanceReport.Grouping);
          Assert.AreEqual(balanceId, specificBalanceReport.BalanceId);
          Assert.AreEqual("balance-report", specificBalanceReport.Resource);
          Assert.AreEqual(from, specificBalanceReport.From);
          Assert.AreEqual(until, specificBalanceReport.Until);
          Assert.IsNotNull(specificBalanceReport.Totals);
          Assert.AreEqual("5.30", specificBalanceReport.Totals.Open.Pending.Amount.Value);
          Assert.AreEqual("EUR", specificBalanceReport.Totals.Open.Pending.Amount.Currency);
          Assert.AreEqual("0.11", specificBalanceReport.Totals.Open.Available.Amount.Value);
          Assert.AreEqual("EUR", specificBalanceReport.Totals.Open.Available.Amount.Currency);
          var childSubTotals = specificBalanceReport.Totals.Payments.Pending.Subtotals.First();
          Assert.AreEqual("payment", childSubTotals.TransactionType);
          Assert.AreEqual(36, childSubTotals.Count);
          var childChildSubTotals = childSubTotals.Subtotals.First();
          Assert.AreEqual("ideal", childChildSubTotals.Method);
      }
      
      [Test]
      public async Task GetBalanceReportAsync_StatusBalances_ResponseIsParsed() {
          // Given: We request a balance report
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          DateTime from = new DateTime(2022, 11, 1);
          DateTime until = new DateTime(2022, 11, 30);
          string grouping = ReportGrouping.StatusBalances;
          
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/report" +
                               $"?from={from.ToString("yyyy-MM-dd")}&until={until.ToString("yyyy-MM-dd")}&grouping={grouping}";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultGetBalanceReportStatusBalancesResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceReport = await balanceClient.GetBalanceReportAsync(balanceId, from, until, grouping);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceReport);
          Assert.AreEqual(typeof(StatusBalanceReportResponse), balanceReport.GetType());
          var specificBalanceReport = (StatusBalanceReportResponse)balanceReport;
          Assert.AreEqual(grouping, specificBalanceReport.Grouping);
          Assert.AreEqual(balanceId, specificBalanceReport.BalanceId);
          Assert.AreEqual("balance-report", specificBalanceReport.Resource);
          Assert.AreEqual(from, specificBalanceReport.From);
          Assert.AreEqual(until, specificBalanceReport.Until);
          Assert.IsNotNull(specificBalanceReport.Totals);
          Assert.AreEqual("5.30", specificBalanceReport.Totals.PendingBalance.Open.Amount.Value);
          Assert.AreEqual("EUR", specificBalanceReport.Totals.PendingBalance.Open.Amount.Currency);
          Assert.AreEqual("3.38", specificBalanceReport.Totals.AvailableBalance.MovedFromPending.Amount.Value);
          var childSubTotals = specificBalanceReport.Totals.AvailableBalance.MovedFromPending.Subtotals.First();
          Assert.AreEqual("payment", childSubTotals.TransactionType);
          var chilcChildSubtotals = childSubTotals.Subtotals.First();
          Assert.AreEqual("ideal", chilcChildSubtotals.Method);
      }
      
      [Test]
      public async Task ListBalanceTransactionsAsync_StatusBalances_ResponseIsParsed() {
          // Given
          string balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/{balanceId}/transactions";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalanceTransactionsResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceTransactions = await balanceClient.ListBalanceTransactionsAsync(balanceId);

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceTransactions?.Embedded?.BalanceTransactions);
          Assert.AreEqual(balanceTransactions.Embedded.BalanceTransactions.Count(), balanceTransactions.Count);
          var transaction = balanceTransactions.Embedded.BalanceTransactions.First();
          Assert.AreEqual("balance_transactions", transaction.Resource);
          Assert.AreEqual("baltr_9S8yk4FFqqi2Qm6K3rqRH", transaction.Id);
          Assert.AreEqual("outgoing-transfer", transaction.Type);
          Assert.AreEqual("-7.76", transaction.ResultAmount.Value);
          Assert.AreEqual("EUR", transaction.ResultAmount.Currency);
          Assert.AreEqual("-7.76", transaction.InitialAmount.Value);
          Assert.AreEqual("EUR", transaction.InitialAmount.Currency);
          Assert.AreEqual(typeof(SettlementBalanceTransaction), transaction.GetType());
          var transactionContext = (SettlementBalanceTransaction)transaction;
          Assert.AreEqual("stl_ma2vu8", transactionContext.Context.SettlementId);
          Assert.AreEqual("trf_ma2vu8", transactionContext.Context.TransferId);
      }
      
      [Test]
      public async Task ListPrimaryBalanceTransactionsAsync_StatusBalances_ResponseIsParsed() {
          // Given
          string expectedUrl = $"{BaseMollieClient.ApiEndPoint}balances/primary/transactions";
          var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultListBalanceTransactionsResponse);
          HttpClient httpClient = mockHttp.ToHttpClient();
          BalanceClient balanceClient = new BalanceClient("api-key", httpClient);

          // When: We make the request
          var balanceTransactions = await balanceClient.ListPrimaryBalanceTransactionsAsync();

          // Then: Response should be parsed
          mockHttp.VerifyNoOutstandingExpectation();
          Assert.IsNotNull(balanceTransactions?.Embedded?.BalanceTransactions);
          Assert.AreEqual(balanceTransactions.Embedded.BalanceTransactions.Count(), balanceTransactions.Count);
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