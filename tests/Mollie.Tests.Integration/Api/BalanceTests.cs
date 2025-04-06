using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

[Trait("TestCategory", "LocalIntegrationTests")]
public class BalanceTests : BaseMollieApiTestClass, IDisposable {
    private readonly IBalanceClient _balanceClient;

    public BalanceTests(IBalanceClient balanceClient) {
        _balanceClient = balanceClient;
    }

    [Fact]
    public async Task GetPrimaryBalanceAsync_IsParsedCorrectly() {
        // When: We retrieve the primary balance from the Mollie API
        var result = await _balanceClient.GetPrimaryBalanceAsync();

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Resource.ShouldBe("balance");
        result.Currency.ShouldNotBeNull();
        result.Id.ShouldNotBeNull();
        result.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/balances-api/get-primary-balance");
        result.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/balances/{result.Id}");
        result.TransferFrequency.ShouldNotBeNull();
        result.AvailableAmount.ShouldNotBeNull();
        result.PendingAmount.ShouldNotBeNull();
        result.TransferThreshold.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBalanceAsync_IsParsedCorrectly() {
        // Given: We get a balance id from the list balances endpoint
        var balanceList = await _balanceClient.GetBalanceListAsync();
        if (balanceList.Count == 0) {
            Assert.Fail("No balance found to retrieve");
        }
        var firstBalance = balanceList.Items.First();

        // When: We retrieve a specific balance from the Mollie API
        var result = await _balanceClient.GetBalanceAsync(firstBalance.Id);

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Resource.ShouldBe("balance");
        result.AvailableAmount.ShouldBe(firstBalance.AvailableAmount);
        result.Id.ShouldBe(firstBalance.Id);
        result.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/balances-api/get-balance");
        result.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/balances/{result.Id}");
        result.Currency.ShouldBe(firstBalance.Currency);
        result.TransferFrequency.ShouldBe(firstBalance.TransferFrequency);
        result.AvailableAmount.ShouldBe(firstBalance.AvailableAmount);
        result.PendingAmount.ShouldBe(firstBalance.PendingAmount);
        result.TransferThreshold.ShouldBe(firstBalance.TransferThreshold);
    }

    [Fact]
    public async Task ListBalancesAsync_IsParsedCorrectly() {
        // When: We retrieve the list of balances
        var result = await _balanceClient.GetBalanceListAsync();

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(result.Count);
    }

    [Theory]
    [InlineData(ReportGrouping.TransactionCategories, typeof(TransactionCategoriesReportResponse))]
    [InlineData(ReportGrouping.StatusBalances, typeof(StatusBalanceReportResponse))]
    public async Task GetBalanceReportAsync_IsParsedCorrectly(string grouping, Type expectedObjectType) {
        // Given: We retrieve the primary balance
        var from = new DateTime(2022, 11, 1);
        var until = new DateTime(2022, 11, 30);
        var primaryBalance = await _balanceClient.GetPrimaryBalanceAsync();

        // When: We retrieve the primary balance report
        var result = await _balanceClient.GetBalanceReportAsync(
            balanceId: primaryBalance.Id,
            from: from,
            until: until,
            grouping: grouping);

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.ShouldBeOfType(expectedObjectType);
        result.Resource.ShouldBe("balance-report");
        result.BalanceId.ShouldBe(primaryBalance.Id);
        result.From.ShouldBe(from);
        result.Until.ShouldBe(until);
        result.Grouping.ShouldBe(grouping);
    }

    [Fact]
    public async Task ListBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;

        // When: We list the balance transactions
        var result = await _balanceClient.GetBalanceTransactionListAsync(balanceId, from, limit);

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeNull();
        result.Links.ShouldNotBeNull();
        result.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/balances/{balanceId}/transactions?from={from}&limit={limit}");
    }

    [Fact]
    public async Task ListPrimaryBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;

        // When: We list the balance transactions
        var result = await _balanceClient.GetPrimaryBalanceTransactionListAsync(from, limit);

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeNull();
    }

    public void Dispose()
    {
        _balanceClient?.Dispose();
    }
}
