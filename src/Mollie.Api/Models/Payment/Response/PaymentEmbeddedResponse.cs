using System.Collections.Generic;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.Refund.Response;

namespace Mollie.Api.Models.Payment.Response {
    public record PaymentEmbeddedResponse {
        public IEnumerable<RefundResponse>? Refunds { get; set; }
        public IEnumerable<ChargebackResponse>? Chargebacks { get; set; }
    }
}
