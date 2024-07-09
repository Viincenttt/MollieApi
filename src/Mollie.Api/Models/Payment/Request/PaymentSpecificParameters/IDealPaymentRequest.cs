using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record IdealPaymentRequest : PaymentRequest {
        public IdealPaymentRequest() {
            Method = PaymentMethod.Ideal;
        }

        [SetsRequiredMembers]
        public IdealPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.Ideal;
        }

        /// <summary>
        /// (Optional) iDEAL issuer id. The id could for example be ideal_INGBNL2A. The returned paymentUrl will then directly
        /// point to the ING web site.
        /// </summary>
        public string? Issuer { get; set; }
    }
}
