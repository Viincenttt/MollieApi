using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payout.Request;
using Mollie.Api.Models.Payout.Response;
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
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;

        var request = new PayoutRequest {
            BalanceId = primaryBalance.Id
        };

        // When: We create a payout
        var result = await _payoutClient.CreatePayoutAsync(request);
        var payout = result.Data!;

        // Then: Make sure we can parse the result
        result.Success.ShouldBeTrue();
        payout.ShouldNotBeNull();
        payout.Resource.ShouldBe("payout");
        payout.Id.ShouldNotBeNullOrEmpty();
        payout.BalanceId.ShouldBe(primaryBalance.Id);
        payout.Status.ShouldNotBeNullOrEmpty();
        payout.StatusReason.ShouldNotBeNull();
        payout.StatusReason.Code.ShouldNotBeNullOrEmpty();
        payout.StatusReason.Message.ShouldNotBeNullOrEmpty();
        payout.CreatedAt.ShouldNotBe(default);
        payout.Mode.ShouldBeOneOf(Mode.Live, Mode.Test);
        payout.Links.ShouldNotBeNull();
        payout.Links.Self.ShouldNotBeNull();
        payout.Links.Self.Href.ShouldNotBeNullOrEmpty();
        payout.Links.Documentation.ShouldNotBeNull();
        payout.Links.Documentation.Href.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreatePayoutAsync_WithSpecificAmountAndDescription_IsParsedCorrectly() {
        // Given: We retrieve the primary balance
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;

        var request = new PayoutRequest {
            BalanceId = primaryBalance.Id,
            Amount = new Amount(Currency.EUR, "10.00"),
            Description = "Integration test payout"
        };

        // When: We create a payout with an amount and description
        var result = await _payoutClient.CreatePayoutAsync(request);
        var payout = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        payout.ShouldNotBeNull();
        payout.Resource.ShouldBe("payout");
        payout.Id.ShouldNotBeNullOrEmpty();
        payout.BalanceId.ShouldBe(primaryBalance.Id);
        payout.Description.ShouldBe("Integration test payout");
        payout.Amount.ShouldNotBeNull();
        payout.Amount!.Currency.ShouldBe("EUR");
    }

    [Fact]
    public async Task GetPayoutListAsync_WithoutParameters_IsParsedCorrectly() {
        // When: We retrieve the list of payouts
        var result = await _payoutClient.GetPayoutListAsync();
        var payouts = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        payouts.ShouldNotBeNull();
        payouts.Items.ShouldNotBeNull();
        payouts.Links.ShouldNotBeNull();
        payouts.Links.Self.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetPayoutListAsync_FilteredByBalanceId_IsParsedCorrectly() {
        // Given: We retrieve the primary balance
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;

        // When: We retrieve payouts filtered by balance ID
        var result = await _payoutClient.GetPayoutListAsync(balanceId: primaryBalance.Id);
        var payouts = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        payouts.ShouldNotBeNull();
        payouts.Items.ShouldNotBeNull();
        foreach (var payout in payouts.Items) {
            payout.BalanceId.ShouldBe(primaryBalance.Id);
        }
    }

    [Fact]
    public async Task GetPayoutAsync_WithValidPayoutId_IsParsedCorrectly() {
        // Given: We create a payout first to get a valid ID
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;
        var createdResult = await _payoutClient.CreatePayoutAsync(new PayoutRequest { BalanceId = primaryBalance.Id });
        var created = createdResult.Data!;

        // When: We retrieve the payout by ID
        var result = await _payoutClient.GetPayoutAsync(created.Id);
        var payout = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        payout.ShouldNotBeNull();
        payout.Resource.ShouldBe("payout");
        payout.Id.ShouldBe(created.Id);
        payout.BalanceId.ShouldBe(primaryBalance.Id);
        payout.Status.ShouldNotBeNullOrEmpty();
        payout.StatusReason.ShouldNotBeNull();
        payout.CreatedAt.ShouldNotBe(default);
        payout.Mode.ShouldBeOneOf(Mode.Live, Mode.Test);
        payout.Links.ShouldNotBeNull();
        payout.Links.Self.Href.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task CancelPayoutAsync_WithRequestedPayout_ReturnsCanceledPayout() {
        // Given: We create a payout first
        var primaryBalanceResult = await _balanceClient.GetPrimaryBalanceAsync();
        var primaryBalance = primaryBalanceResult.Data!;
        var createdResult = await _payoutClient.CreatePayoutAsync(new PayoutRequest { BalanceId = primaryBalance.Id });
        var created = createdResult.Data!;

        // When: We cancel the payout while it is still in 'requested' status
        var result = await _payoutClient.CancelPayoutAsync(created.Id);
        var payout = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        payout.ShouldNotBeNull();
        payout.Resource.ShouldBe("payout");
        payout.Id.ShouldBe(created.Id);
        payout.Status.ShouldBe(PayoutStatus.Canceled);
        payout.CanceledAt.ShouldNotBeNull();
    }

    public void Dispose() {
        _payoutClient?.Dispose();
        _balanceClient?.Dispose();
    }
}

