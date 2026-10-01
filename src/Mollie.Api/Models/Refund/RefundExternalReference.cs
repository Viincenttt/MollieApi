namespace Mollie.Api.Models.Refund;

public record RefundExternalReference {
    /// <summary>
    /// Specifies the reference type. See the Mollie.Api.Models.Refund.RefundExternalReferenceType class for a full
    /// list of known values.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Unique reference from the payment provider, for example 123456789012345.
    /// </summary>
    public required string Id { get; set; }
}
