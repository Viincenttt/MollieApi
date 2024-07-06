using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Refund;

public record RoutingReversal {
    /// <summary>
    /// The amount that will be pulled back.
    /// </summary>
    public required Amount Amount { get; init; }

    /// <summary>
    /// Where the funds will be pulled back from.
    /// </summary>
    public required RoutingDestination Source { get; init; }
}
