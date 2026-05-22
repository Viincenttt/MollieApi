using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payout.Request;
using Mollie.Tests.Integration.Framework;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

[Trait("TestCategory", "LocalIntegrationTests")]
public class PayoutTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPayoutClient _payoutClient;
    private readonly IBalanceClient _balanceClient;

    public PayoutTests(IPayoutClient payoutClient, IBalanceClient balanceClient) {
        _payoutClient = payoutClient;
        _balanceClient = balanceClient;
    }

    [Fact]
    public async Task CreatePayoutAsync_WithFullBalance_IsParsedCorrectly() {
        // Given: We retrieve the primary balance to get a valid balance ID
        var primaryBalance = await _balanceClient.GetPrimaryBalanceAsync();

        var request = new PayoutRequest {
            BalanceId = primaryBalance.Id
        };

        // When: We create a payout
        var result = await _payoutClient.CreatePayoutAsync(request);

        // Then: Make sure we can parse the result
        result.ShouldNotBeNull();
        result.Resource.ShouldBe("payout");
        result.Id.ShouldNotBeNullOrEmpty();
        result.BalanceId.ShouldBe(primaryBalance.Id);
        result.Status.ShouldNotBeNullOrEmpty();
        result.StatusReason.ShouldNotBeNull();
        result.StatusReason.Code.ShouldNotBeNullOrEmpty();
        result.StatusReason.Message.ShouldNotBeNullOrEmpty();
        result.CreatedAt.ShouldNotBe(default);
        result.Mode.ShouldBeOneOf(Mode.Live, Mode.Test);
        result.Links.ShouldNotBeNull();
        result.Links.Self.ShouldNotBeNull();
        result.Links.Self.Href.ShouldNotBeNullOrEmpty();
        result.Links.Documentation.ShouldNotBeNull();
        result.Links.Documentation.Href.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreatePayoutAsync_WithSpecificAmountAndDescription_IsParsedCorrectly() {
        // Given: We retrieve the primary balance
        var primaryBalance = await _balanceClient.GetPrimaryBalanceAsync();

        var request = new PayoutRequest {
            BalanceId = primaryBalance.Id,
            Amount = new Amount(Currency.EUR, "10.00"),
            Description = "Integration test payout"
        };

        // When: We create a payout with an amount and description
        var result = await _payoutClient.CreatePayoutAsync(request);

        // Then
        result.ShouldNotBeNull();
        result.Resource.ShouldBe("payout");
        result.Id.ShouldNotBeNullOrEmpty();
        result.BalanceId.ShouldBe(primaryBalance.Id);
        result.Description.ShouldBe("Integration test payout");
        result.Amount.ShouldNotBeNull();
        result.Amount!.Currency.ShouldBe("EUR");
    }

    [Fact]
    public async Task GetPayoutListAsync_WithoutParameters_IsParsedCorrectly() {
        // When: We retrieve the list of payouts
        var result = await _payoutClient.GetPayoutListAsync();

        // Then
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeNull();
        result.Links.ShouldNotBeNull();
        result.Links.Self.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetPayoutListAsync_FilteredByBalanceId_IsParsedCorrectly() {
        // Given: We retrieve the primary balance
        var primaryBalance = await _balanceClient.GetPrimaryBalanceAsync();

        // When: We retrieve payouts filtered by balance ID
        var result = await _payoutClient.GetPayoutListAsync(balanceId: primaryBalance.Id);

        // Then
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeNull();
        foreach (var payout in result.Items) {
            payout.BalanceId.ShouldBe(primaryBalance.Id);
        }
    }

    public void Dispose() {
        _payoutClient?.Dispose();
        _balanceClient?.Dispose();
    }
}

