using System.Collections.Generic;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Models.Payment.Response {
    public record PaymentEmbeddedResponse : IResponseObject {
        public IEnumerable<RefundResponse>? Refunds { get; set; }
        public IEnumerable<ChargebackResponse>? Chargebacks { get; set; }
    }
}