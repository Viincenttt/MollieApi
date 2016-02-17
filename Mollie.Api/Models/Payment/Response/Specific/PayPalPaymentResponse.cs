namespace Mollie.Api.Models.Payment.Response {
    public class PayPalPaymentResponse : PaymentResponse {
        public PayPalPaymentResponseDetails Details { get; set; }
    }

    public class PayPalPaymentResponseDetails {
        public string PayPalReference { get; set; }

        public string CustomerReference { get; set; }
    }
}