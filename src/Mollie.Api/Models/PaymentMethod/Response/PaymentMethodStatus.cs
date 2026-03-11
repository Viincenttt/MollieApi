namespace Mollie.Api.Models.PaymentMethod.Response;

/// <summary>
/// The payment method's activation status for this profile.
/// </summary>
public static class PaymentMethodStatus {
    public const string Activated = "activated";
    public const string PendingBoarding = "pending-boarding";
    public const string PendingReview = "pending-review";
    public const string PendingExternal = "pending-external";
    public const string Rejected = "rejected";
}
