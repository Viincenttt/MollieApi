using System;
using System.Text.Json;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Subscription.Response {
    public record SubscriptionResponse {
        /// <summary>
        /// Indicates the response contains a subscription object.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The subscription's unique identifier, for example sub_rVKGtNd6s3.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The mode used to create this subscription. Mode determines whether the payments are real or test payments.
        /// </summary>
        public Mode Mode { get; set; }

        /// <summary>
        ///  The subscription's date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The subscription's current status, depends on whether the customer has a pending, valid or invalid mandate.
        /// See the Mollie.Api.Models.Subscription.SubscriptionStatus class for a full list of known values.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// The constant amount that is charged with each subscription payment.
        /// </summary>
        public required Amount Amount { get; set; }

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
        public required string Interval { get; set; }

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
        public required string Description { get; set; }

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
        /// The customer this subscription belongs to.
        /// </summary>
        public required string CustomerId { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the subscription. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required SubscriptionResponseLinks Links { get; set; }

        /// <summary>
        /// Adding an application fee allows you to charge the merchant for each payment in the subscription and
        /// transfer these amounts to your own account.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }

        public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
            return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
        }
    }
}
