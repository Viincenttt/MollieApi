namespace Mollie.Api.Models.Payment {
    public enum PaymentStatus {
        Open,
        Canceled,
        Pending,
        Paid,
        PaidOut,
        Refunded,
        Expired,
        Failed,
        Charged_Back
    }
}