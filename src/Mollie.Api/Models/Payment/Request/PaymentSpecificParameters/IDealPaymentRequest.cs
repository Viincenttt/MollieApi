using System;
using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    [Obsolete("Ideal no longer has specific parameters, so this class is identical to PaymentRequest")]
    public record IdealPaymentRequest : PaymentRequest {
        public IdealPaymentRequest() {
            Method = PaymentMethod.Ideal;
        }

        [SetsRequiredMembers]
        public IdealPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.Ideal;
        }
    }
}
