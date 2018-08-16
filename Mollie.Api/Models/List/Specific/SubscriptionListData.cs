using System.Collections.Generic;
using Mollie.Api.Models.Subscription;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class SubscriptionListData : IListData<SubscriptionResponse> {
        [JsonProperty("subscriptions")]
        public List<SubscriptionResponse> Items { get; set; }
    }
}