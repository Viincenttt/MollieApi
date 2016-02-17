using System;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Models.Refund {
    public class RefundResponse {
        public string Id { get; set; }
        public PaymentResponse Payment { get; set; }
        public decimal AmountRefunded { get; set; }
        public decimal AmountRemaining { get; set; }
        public DateTime? RefundedDatetime { get; set; }

        public override string ToString() {
            return $"Id: {this.Id} - PaymentId: {Payment.Id}";
        }
    }
}
