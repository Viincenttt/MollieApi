namespace Mollie.Api.Models.BalanceTransfer.Response;

public class BalanceTransferStatusReason {
    /// <summary>
    /// A machine-readable code that indicates the reason for the transfer's status.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// A description of the status reason, localized according to the transfer.
    /// </summary>
    public required string Message { get; set; }
}
