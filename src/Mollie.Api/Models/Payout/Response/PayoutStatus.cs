namespace Mollie.Api.Models.Payout.Response;

public static class PayoutStatus {
    public const string Requested = "requested";
    public const string Initiated = "initiated";
    public const string ProcessingAtBank = "processing-at-bank";
    public const string Completed = "completed";
    public const string Failed = "failed";
    public const string Canceled = "canceled";
}

