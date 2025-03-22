namespace Mollie.Api.Models.Payment.Response;

public record StatusReason {
    /// <summary>
    /// A machine-readable code that indicates the reason for the payment's status.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// A machine-readable code that indicates the reason for the payment's status.
    /// </summary>
    public required string Message { get; set; }
}
