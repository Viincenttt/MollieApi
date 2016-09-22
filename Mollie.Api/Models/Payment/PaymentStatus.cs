namespace Mollie.Api.Models.Payment {
    public enum PaymentStatus {
        Open,
        Cancelled,
        Pending,
        Paid,
        PaidOut,
        Refunded,
        Expired,
        Failed,
        Charged_Back
    }
}
