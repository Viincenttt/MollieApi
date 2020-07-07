namespace Mollie.Api.Models.Refund {
    public static class RefundStatus {
        public const string Pending = nameof(Pending);
        public const string Processing = nameof(Processing);
        public const string Refunded = nameof(Refunded);
        public const string Queued = nameof(Queued);
        public const string Failed = nameof(Failed);
    }
}