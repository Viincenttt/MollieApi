using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Refund {
    public class RefundResponse : IResponseObject {
        /// <summary>
        /// Indicates the response contains a refund object. Will always contain refund for this endpoint.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The refund's unique identifier, for example re_4qqhO89gsT.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The amount refunded to the consumer with this refund.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// This optional field will contain the amount that will be deducted from your account balance, converted 
        /// to the currency your account is settled in. It follows the same syntax as the amount property. Note that
        /// for refunds, the value key of settlementAmount will be negative. Any amounts not settled by Mollie will 
        /// not be reflected in this amount, e.g. PayPal refunds.
        /// </summary>
        public Amount SettlementAmount { get; set; }

        /// <summary>
        /// The description of the refund that may be shown to the consumer, depending on the payment method used.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Since refunds may be delayed for certain payment methods, the refund carries a status field.
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// The date and time the refund was issued, in ISO 8601 format.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The unique identifier of the payment this refund was created for. For example: tr_7UhSN1zuXS. The full 
        /// payment object can be retrieved via the payment URL in the _links object.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the refund. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public RefundResponseLinks Links { get; set; }

        public override string ToString() {
            return $"Id: {this.Id} - PaymentId: {this.PaymentId}";
        }
    }
}