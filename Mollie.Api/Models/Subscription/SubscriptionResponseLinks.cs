using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponseLinks {
        /// <summary>
        ///     The API resource URL of the subscription itself.
        /// </summary>
        public UrlObjectLink<SubscriptionResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the subscription is for.
        /// </summary>
        public UrlObjectLink<CustomerResponse> Customer { get; set; }

        /// <summary>
        /// The URL to the subscription retrieval endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
    }
}