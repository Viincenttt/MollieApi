using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Refund;

public record RoutingReversal {
    /// <summary>
    /// The amount to refund to the source
    /// </summary>
    public required Amount Amount { get; init; }

    /// <summary>
    /// The source to which the amount should be refunded
    /// </summary>
    public required RoutingDestination Source { get; init; }
}
