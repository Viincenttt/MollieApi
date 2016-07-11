namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponseLinks {
        /// <summary>
        /// The URL Mollie will call as soon a payment status change takes place.
        /// </summary>
        public string WebhookUrl { get; set; }
    }
}
