namespace Mollie.Api.Models.Payment {
    public static class PaymentStatus {
        public const string Open = "open";
        public const string Canceled = "canceled";
        public const string Pending = "pending";
        public const string Authorized = "authorized";
        public const string Expired = "expired";
        public const string Failed = "failed";
        public const string Paid = "paid";
    }
}