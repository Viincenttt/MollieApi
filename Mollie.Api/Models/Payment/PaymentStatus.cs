namespace Mollie.Api.Models.Payment {
    public enum PaymentStatus {
        Open,
        Cancelled,
        Pending,
        Authorized,
        Expired,
        Failed,
        Paid
    }
}
