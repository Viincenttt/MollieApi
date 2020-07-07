using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a subscription object.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The subscription's unique identifier, for example sub_rVKGtNd6s3.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The mode used to create this subscription. Mode determines whether the payments are real or test payments.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Mode Mode { get; set; }
        
        /// <summary>
        ///  The subscription's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// The subscription's current status, depends on whether the customer has a pending, valid or invalid mandate.
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// The constant amount that is charged with each subscription payment.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Total number of charges for the subscription to complete.
        /// </summary>
        public int? Times { get; set; }

        /// <summary>
        /// Interval to wait between charges like 1 month(s) or 14 days.
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// The start date of the subscription in yyyy-mm-dd format.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date of the next scheduled payment in YYYY-MM-DD format. When there will be no next payment, for example
        /// when the subscription has ended, this parameter will not be returned.
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// A description unique per customer. This will be included in the payment description along with the charge date in
        /// Y-m-d format.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The payment method used for this subscription, either forced on creation by specifying the method parameter, or
        /// null if any of the customer's valid mandates may be used.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public string Method { get; set; }

        /// <summary>
        /// The subscription's date of cancellation, in ISO 8601 format.
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon a payment status change takes place.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the subscription. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public SubscriptionResponseLinks Links { get; set; }

        public T GetMetadata<T>(JsonSerializerSettings jsonSerializerSettings = null) {
            return JsonConvert.DeserializeObject<T>(this.Metadata, jsonSerializerSettings);
        }
    }
}