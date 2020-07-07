using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Subscription {
    public class SubscriptionRequest : SubscriptionUpdateRequest {
        /// <summary>
        /// Interval to wait between charges like 1 month(s) or 14 days.
        /// </summary>
        public string Interval { get; set; }
        
        /// <summary>
        /// Optional – The payment method used for this subscription, either forced on creation or null if any of the
        /// customer's valid mandates may be used.
        /// </summary>
        public string Method { get; set; }
    }
}