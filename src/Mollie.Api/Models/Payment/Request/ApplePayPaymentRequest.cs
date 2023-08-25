namespace Mollie.Api.Models.Payment.Request {
    public class ApplePayPaymentRequest : PaymentRequest {
        public ApplePayPaymentRequest()
        {
            this.Method = PaymentMethod.ApplePay;
        }

        /// <summary>
        /// Optional - The Apple Pay Payment Token object (encoded as JSON) that is part of the result of authorizing a payment
        /// request. The token contains the payment information needed to authorize the payment.
        /// </summary>
        public string ApplePayPaymentToken { get; set; }
    }
}