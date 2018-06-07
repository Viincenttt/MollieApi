using System.Collections.Generic;
using Mollie.Api.Models.Subscription;

namespace Mollie.Api.Models.List.Specific {
    public class SubscriptionListData {
        public List<SubscriptionResponse> Subscriptions { get; set; }
    }
}