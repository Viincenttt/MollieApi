using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment.Response {
    using JsonConverters;

    public class PaymentResponse {
        /// <summary>
        /// The identifier uniquely referring to this payment. Mollie assigns this identifier randomly at payment creation time. For example tr_7UhSN1zuXS. 
        /// Its ID will always be used by Mollie to refer to a certain payment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
        /// </summary>
        public PaymentMode Mode { get; set; }

        /// <summary>
        /// The payment's date and time of creation, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedDatetime { get; set; }

        /// <summary>
        /// The payment's status. Please refer to the page about statuses for more info about which statuses occur at what point.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentStatus? Status { get; set; }

        /// <summary>
        /// The date and time the payment became paid, in ISO 8601 format. Null is returned if the payment isn't completed (yet).
        /// </summary>
        public DateTime? PaidDatetime { get; set; }

        /// <summary>
        /// The date and time the payment was cancelled, in ISO 8601 format. Null is returned if the payment isn't cancelled (yet).
        /// </summary>
        public DateTime? CancelledDatetime { get; set; }

        /// <summary>
        /// The date and time the payment was expired, in ISO 8601 format. Null is returned if the payment did not expire (yet).
        /// </summary>
        public DateTime? ExpiredDatetime { get; set; }

        /// <summary>
        /// The time until the payment will expire in ISO 8601 duration format.
        /// </summary>
        public string ExpiryPeriod { get; set; }

        /// <summary>
        /// The amount in EURO.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The total amount in EURO that is already refunded. For some payment methods, this 
        /// amount may be higher than the payment amount, for example to allow reimbursement of the costs for a return shipment to the consumer.
        /// </summary>
        public decimal? AmountRefunded { get; set; }

        /// <summary>
        /// Only available when refunds are available for this payment – The remaining amount in EURO that can be refunded.
        /// </summary>
        public decimal? AmountRemaining { get; set; }

        /// <summary>
        /// A short description of the payment. The description will be shown on the consumer's bank or card statement when possible.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The payment method used for this payment, either forced on creation by specifying the method parameter, or chosen by the consumer our 
        /// payment method selection screen.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMethod? Method { get; set; }

        /// <summary>
        /// The optional metadata you provided upon payment creation. Metadata can be used to link an order to a payment.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }

        public T GetMetadata<T>(JsonSerializerSettings jsonSerializerSettings = null) {
            return JsonConvert.DeserializeObject<T>(this.Metadata, jsonSerializerSettings);
        }

        /// <summary>
        /// The consumer's locale, either forced on creation by specifying the locale parameter, or detected by us during checkout.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The identifier referring to the profile this payment was created on. For example, pfl_QkEhN94Ba.
        /// </summary>  
        public string ProfileId { get; set; }

        /// <summary>
        /// The identifier referring to the settlement this payment belongs to. For example, stl_BkEjN2eBb.
        /// </summary>
        public string SettlementId { get; set; }

        /// <summary>
        /// The customerid of this payment
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// The recurringtype of this payment
        /// </summary>
        public RecurringType? RecurringType { get; set; }

        /// <summary>
        /// An object with several URLs important to the payment process.
        /// </summary>
        public PaymentResponseLinks Links { get; set; }

        /// <summary>
        /// Only available for recurring payments – If the payment is a recurring payment, this field will hold the ID of the mandate used to authorize the recurring payment.
        /// </summary>
        public string MandateId { get; set; }

        /// <summary>
        /// Only available for recurring payments – When implementing the Subscriptions API, any recurring charges resulting from the subscription will hold the ID of the subscription that triggered the payment.
        /// </summary>
        public string SubscriptionId { get; set; }

        public override string ToString() {
            return $"Id: {this.Id} - Status: {this.Status} - Method: {this.Method} - Amount: {this.Amount}";
        }
    }
}
