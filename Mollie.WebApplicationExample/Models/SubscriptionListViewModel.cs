using System.Collections.Generic;
using Mollie.Api.Models.Subscription;

namespace Mollie.WebApplicationExample.Models {
    public class SubscriptionListViewModel {
        public string CustomerId { get; set; }
        public IList<SubscriptionResponse> Subscriptions { get; set; }
    }
}