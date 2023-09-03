using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api; 

[Trait("TestCategory", "AccessKeyIntegrationTest")]
public class BalanceTests : BaseMollieApiTestClass, IDisposable {
    private readonly IBalanceClient _balanceClient;

    public BalanceTests() {
        _balanceClient = new BalanceClient(this.ApiKey);
    }
    
    [DefaultRetryFact]
    public async Task GetPrimaryBalanceAsync_IsParsedCorrectly() {
        // When: We retrieve the primary balance from the Mollie API
        var result = await _balanceClient.GetPrimaryBalanceAsync();

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Resource.Should().Be("balance");
        result.Currency.Should().NotBeNull();
        result.Id.Should().NotBeNull();
        result.Links.Documentation.Href.Should().Be("https://docs.mollie.com/reference/v2/balances-api/get-primary-balance");
        result.Links.Self.Href.Should().Be("https://api.mollie.com/v2/balances/{result.Id}");
        result.TransferFrequency.Should().NotBeNull();
        result.AvailableAmount.Should().NotBeNull();
        result.PendingAmount.Should().NotBeNull();
        result.TransferThreshold.Should().NotBeNull();
    }
        
    [DefaultRetryFact]
    public async Task GetBalanceAsync_IsParsedCorrectly() {
        // Given: We get a balance id from the list balances endpoint
        var balanceList = await this._balanceClient.ListBalancesAsync();
        if (balanceList.Count == 0) {
            Assert.Fail("No balance found to retrieve");
        }
        var firstBalance = balanceList.Items.First();

        // When: We retrieve a specific balance from the Mollie API
        var result = await this._balanceClient.GetBalanceAsync(firstBalance.Id);

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Resource.Should().Be("balance");
        result.AvailableAmount.Should().Be(firstBalance.AvailableAmount);
        result.Id.Should().Be(firstBalance.Id);
        result.Links.Documentation.Href.Should().Be("https://docs.mollie.com/reference/v2/balances-api/get-balance");
        result.Links.Self.Href.Should().Be($"https://api.mollie.com/v2/balances/{result.Id}");
        result.Currency.Should().Be(firstBalance.Currency);
        result.TransferFrequency.Should().Be(firstBalance.TransferFrequency);
        result.AvailableAmount.Should().Be(firstBalance.AvailableAmount);
        result.PendingAmount.Should().Be(firstBalance.PendingAmount);
        result.TransferThreshold.Should().Be(firstBalance.TransferThreshold);
    }
        
    [DefaultRetryFact]
    public async Task ListBalancesAsync_IsParsedCorrectly() {
        // When: We retrieve the list of balances
        var result = await this._balanceClient.ListBalancesAsync();

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Items.Count.Should().Be(result.Count);
    }
        
    [DefaultRetryTheory]
    [InlineData(ReportGrouping.TransactionCategories, typeof(TransactionCategoriesReportResponse))]
    [InlineData(ReportGrouping.StatusBalances, typeof(StatusBalanceReportResponse))]
    public async Task GetBalanceReportAsync_IsParsedCorrectly(string grouping, Type expectedObjectType) {
        // Given: We retrieve the primary balance
        var from = new DateTime(2022, 11, 1);
        var until = new DateTime(2022, 11, 30);
        var primaryBalance = await this._balanceClient.GetPrimaryBalanceAsync();
            
        // When: We retrieve the primary balance report
        var result = await this._balanceClient.GetBalanceReportAsync(
            balanceId: primaryBalance.Id,
            from: from, 
            until: until,
            grouping: grouping);

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedObjectType);
        result.Resource.Should().Be("balance-report");
        result.BalanceId.Should().Be(primaryBalance.Id);
        result.From.Should().Be(from);
        result.Until.Should().Be(until);
        result.Grouping.Should().Be(grouping);
    }
        
    [DefaultRetryFact]
    public async Task ListBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var balanceId = "bal_CKjKwQdjCwCSArXFAJNFH";
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;
            
        // When: We list the balance transactions
        var result = await this._balanceClient.ListBalanceTransactionsAsync(balanceId, from, limit);

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Embedded.Should().NotBeNull();
        result.Embedded.BalanceTransactions.Should().NotBeNull();
        result.Links.Should().NotBeNull();
        result.Links.Self.Href.Should().Be($"https://api.mollie.com/v2/balances/{balanceId}/transactions?from={from}&limit={limit}");
    }
        
    [DefaultRetryFact]
    public async Task ListPrimaryBalanceTransactionsAsync_IsParsedCorrectly() {
        // Given
        var from = "baltr_9S8yk4FFqqi2Qm6K3rqRH";
        var limit = 250;
            
        // When: We list the balance transactions
        var result = await this._balanceClient.ListPrimaryBalanceTransactionsAsync(from, limit);

        // Then: Make sure we can parse the result
        result.Should().NotBeNull();
        result.Embedded.Should().NotBeNull();
        result.Embedded.BalanceTransactions.Should().NotBeNull();
    }

    public void Dispose()
    {
        _balanceClient?.Dispose();
    }
}