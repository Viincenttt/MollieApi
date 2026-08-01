using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
        var balance = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        balance.ShouldNotBeNull();
        balance.Resource.ShouldBe("balance");
        balance.Currency.ShouldNotBeNull();
        balance.Id.ShouldNotBeNull();
        balance.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/balances-api/get-primary-balance");
        balance.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/balances/{balance.Id}");
        balance.TransferFrequency.ShouldNotBeNull();
        balance.AvailableAmount.ShouldNotBeNull();
        balance.PendingAmount.ShouldNotBeNull();
        balance.TransferThreshold.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBalanceAsync_IsParsedCorrectly() {
        // Given: We get a balance id from the list balances endpoint
        var balanceListResult = await _balanceClient.GetBalanceListAsync();
        var balanceList = balanceListResult.Data!;
        if (balanceList.Count == 0) {
            Assert.Fail("No balance found to retrieve");
        }
        var firstBalance = balanceList.Items.First();

        // When: We retrieve a specific balance from the Mollie API
        var result = await _balanceClient.GetBalanceAsync(firstBalance.Id);
        var balance = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        balance.ShouldNotBeNull();
        balance.Resource.ShouldBe("balance");
        balance.AvailableAmount.ShouldBe(firstBalance.AvailableAmount);
        balance.Id.ShouldBe(firstBalance.Id);
        balance.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/balances-api/get-balance");
        balance.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/balances/{balance.Id}");
        balance.Currency.ShouldBe(firstBalance.Currency);
        balance.TransferFrequency.ShouldBe(firstBalance.TransferFrequency);
        balance.AvailableAmount.ShouldBe(firstBalance.AvailableAmount);
        balance.PendingAmount.ShouldBe(firstBalance.PendingAmount);
        balance.TransferThreshold.ShouldBe(firstBalance.TransferThreshold);
    }

    [Fact]
    public async Task ListBalancesAsync_IsParsedCorrectly() {
        // When: We retrieve the list of balances
        var result = await _balanceClient.GetBalanceListAsync();
        var balanceList = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        balanceList.ShouldNotBeNull();
        balanceList.Items.Count.ShouldBe(balanceList.Count);
    }

    [Theory]
    [InlineData(ReportGrouping.TransactionCategories, typeof(TransactionCategoriesReportResponse))]
    [InlineData(ReportGrouping.StatusBalances, typeof(StatusBalanceReportResponse))]
    public async Task GetBalanceReportAsync_IsParsedCorrectly(string grouping, Type expectedObjectType) {
        // Given: We retrieve the primary balance
        var from = new DateTime(2022, 11, 1);
        var until = new DateTime(2022, 11, 30);
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;

        // When: We retrieve the primary balance report
        var result = await _balanceClient.GetBalanceReportAsync(
            balanceId: primaryBalance.Id,
            from: from,
            until: until,
            grouping: grouping);
        var report = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        report.ShouldNotBeNull();
        report.ShouldBeOfType(expectedObjectType);
        report.Resource.ShouldBe("balance-report");
        report.BalanceId.ShouldBe(primaryBalance.Id);
        report.From.ShouldBe(from);
        report.Until.ShouldBe(until);
        report.Grouping.ShouldBe(grouping);
    }

    [Fact]
    public async Task ListBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;

        // When: We list the balance transactions
        var result = await _balanceClient.GetBalanceTransactionListAsync(balanceId, from, limit);
        var transactions = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        transactions.ShouldNotBeNull();
        transactions.Items.ShouldNotBeNull();
        transactions.Links.ShouldNotBeNull();
        transactions.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/balances/{balanceId}/transactions?from={from}&limit={limit}");
    }

    [Fact]
    public async Task ListPrimaryBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;

        // When: We list the balance transactions
        var result = await _balanceClient.GetPrimaryBalanceTransactionListAsync(from, limit);
        var transactions = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        transactions.ShouldNotBeNull();
        transactions.Items.ShouldNotBeNull();
    }

    public void Dispose()
    {
        _balanceClient?.Dispose();
    }
}
