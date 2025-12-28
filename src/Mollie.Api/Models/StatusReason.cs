namespace Mollie.Api.Models;

public record StatusReason {
    /// <summary>
    /// A machine-readable code that indicates the reason for the status.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// A machine-readable code that indicates the reason for the status.
    /// </summary>
    public required string Message { get; set; }
}
