namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponseLinks {
        /// <summary>
        ///     The API resource URL of the subscription itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the subscription is for.
        /// </summary>
        public UrlObject Customer { get; set; }

        /// <summary>
        /// The URL to the subscription retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}