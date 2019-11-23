using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public class PaymentSpecificParameters {
        public string CustomerId { get; set; }
        public SequenceType SequenceType { get; set; }
    }
}
