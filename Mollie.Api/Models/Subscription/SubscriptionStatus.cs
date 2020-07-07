namespace Mollie.Api.Models.Subscription {
    public static class SubscriptionStatus {
        public const string Pending = nameof(Pending);
        public const string Active = nameof(Active);
        public const string Canceled = nameof(Canceled);
        public const string Suspended = nameof(Suspended);
        public const string Completed = nameof(Completed);
    }
}