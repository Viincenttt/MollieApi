namespace Mollie.Api.Models.Payment {
    public static class PaymentStatus {
        public const string Open = nameof(Open);
        public const string Canceled = nameof(Canceled);
        public const string Pending = nameof(Pending);
        public const string Authorized = nameof(Authorized);
        public const string Expired = nameof(Expired);
        public const string Failed = nameof(Failed);
        public const string Paid = nameof(Paid);
    }
}