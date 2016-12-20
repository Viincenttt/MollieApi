using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Subscription {
    public class SubscriptionResponse {
        /// <summary>
        /// Indicates the response contains a subscription object.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The subscription's unique identifier, for example sub_rVKGtNd6s3.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The customer's unique identifier, for example cst_stTC2WHAuS.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// The mode used to create this subscription. Mode determines whether the payments are real or test payments.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Mode Mode { get; set; }

        /// <summary>
        /// The subscription's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime CreatedDatetime { get; set; }

        /// <summary>
        /// The subscription's current status, depends on whether the customer has a pending, valid or invalid mandate.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionStatus Status { get; set; }

        /// <summary>
        /// The constant amount that is charged with each subscription payment.
        /// </summary>
        public decimal Amount { get; set; }

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
        /// A description unique per customer. This will be included in the payment description along with the charge date in Y-m-d format.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The payment method used for this subscription, either forced on creation by specifying the method parameter, or null if any of the customer's valid mandates may be used.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Payment.PaymentMethod Method { get; set; }

        /// <summary>
        /// The subscription's date of cancellation, in ISO 8601 format.
        /// </summary>
        public DateTime? CancelledDateTime { get; set; }

        /// <summary>
        /// An object with URLs important to the payment process.
        /// </summary>
        public SubscriptionResponseLinks Links { get; set; }
    }
}
