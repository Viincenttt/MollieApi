using System;
using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    [Obsolete("Kbc no longer has specific parameters, so this class is identical to PaymentRequest")]
    public record KbcPaymentRequest : PaymentRequest {
        public KbcPaymentRequest() {
            Method = PaymentMethod.Kbc;
        }

        [SetsRequiredMembers]
        public KbcPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.Kbc;
        }
    }
}
