using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record ApplePayPaymentRequest : PaymentRequest {
        public ApplePayPaymentRequest()
        {
            Method = PaymentMethod.ApplePay;
        }

        [SetsRequiredMembers]
        public ApplePayPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.ApplePay;
        }

        /// <summary>
        /// Optional - The Apple Pay Payment Token object (encoded as JSON) that is part of the result of authorizing a payment
        /// request. The token contains the payment information needed to authorize the payment.
        /// </summary>
        public string? ApplePayPaymentToken { get; set; }
    }
}
