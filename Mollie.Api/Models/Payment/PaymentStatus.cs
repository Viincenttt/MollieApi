namespace Mollie.Api.Models.Payment {
    public enum PaymentStatus {
        Open,
        Canceled,
        Pending,
        Authorized,
        Expired,
        Failed,
        Paid
    }
}