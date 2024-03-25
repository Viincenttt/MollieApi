using System;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a subscription object.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The subscription's unique identifier, for example sub_rVKGtNd6s3.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The mode used to create this subscription. Mode determines whether the payments are real or test payments.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Mode Mode { get; set; }
        
        /// <summary>
        ///  The subscription's date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; init; }

        /// <summary>
        /// The subscription's current status, depends on whether the customer has a pending, valid or invalid mandate.
        /// See the Mollie.Api.Models.Subscription.SubscriptionStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; init; }
        
        /// <summary>
        /// The constant amount that is charged with each subscription payment.
        /// </summary>
        public required Amount Amount { get; init; }

        /// <summary>
        /// Total number of charges for the subscription to complete.
        /// </summary>
        public int? Times { get; set; }

        /// <summary>
        /// Number of charges left for the subscription to complete.
        /// </summary>
        public int? TimesRemaining { get; set; }

        /// <summary>
        /// Interval to wait between charges like 1 month(s) or 14 days.
        /// </summary>
        public required string Interval { get; init; }

        /// <summary>
        /// The start date of the subscription in yyyy-mm-dd format.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The date of the next scheduled payment in YYYY-MM-DD format. When there will be no next payment, for example
        /// when the subscription has ended, this parameter will not be returned.
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// A description unique per customer. This will be included in the payment description along with the charge date in
        /// Y-m-d format.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// The payment method used for this subscription, either forced on creation by specifying the method parameter, or
        /// null if any of the customer's valid mandates may be used. See the Mollie.Api.Models.Payment.PaymentMethod class
        /// for a full list of known values.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The mandate used for this subscription. Please note that this parameter can not set together with method.
        /// </summary>
        public string? MandateId { get; set; }

        /// <summary>
        /// The subscription's date of cancellation, in ISO 8601 format.
        /// </summary>
        public DateTime? CanceledAt { get; set; }

        /// <summary>
        /// The URL Mollie will call as soon a payment status change takes place.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the subscription. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required SubscriptionResponseLinks Links { get; init; }

        /// <summary>
        /// Adding an application fee allows you to charge the merchant for each payment in the subscription and 
        /// transfer these amounts to your own account.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }

        public T? GetMetadata<T>(JsonSerializerSettings? jsonSerializerSettings = null) {
            return Metadata != null ? JsonConvert.DeserializeObject<T>(Metadata, jsonSerializerSettings) : default;
        }
    }
}
