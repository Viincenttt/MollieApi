namespace Mollie.Api.Models.Payout.Request;

public record PayoutRequest : ITestModeRequest {
    /// <summary>
    /// The identifier of the balance that will be paid out.
    /// Example: bal_gVMhHKqSSRYJyPsuoPNFH.
    /// </summary>
    public required string BalanceId { get; set; }

    /// <summary>
    /// The amount to pay out. When omitted, the full available balance minus any configured balance reserve is paid out.
    /// </summary>
    public Amount? Amount { get; set; }

    /// <summary>
    /// The description that will appear on the bank statement for this payout.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether to create the entity in test mode or live mode.
    /// </summary>
    public bool? Testmode { get; set; }
}

