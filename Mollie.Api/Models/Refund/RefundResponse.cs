using System;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Models.Refund {
    public class RefundResponse {
        /// <summary>
        /// The refund's unique identifier, for example re_4qqhO89gsT.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The original payment, as described in Get payment. In the payment object, note the following refund related 
        /// fields: AmountRefunded and AmountRemaining
        /// </summary>
        public PaymentResponse Payment { get; set; }

        /// <summary>
        /// The date and time the refund was issued, in ISO 8601 format.
        /// </summary>
        public DateTime? RefundedDatetime { get; set; }

        /// <summary>
        /// The amount refunded to the consumer with this refund.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Since refunds may be delayed for certain payment methods, the refund carries a status field.
        /// </summary>
        public RefundStatus Status { get; set; }

        public override string ToString() {
            return $"Id: {this.Id} - PaymentId: {this.Payment.Id}";
        }
    }
}
