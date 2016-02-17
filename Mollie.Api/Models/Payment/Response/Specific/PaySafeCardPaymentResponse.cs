namespace Mollie.Api.Models.Payment.Response {
    public class PaySafeCardPaymentResponse : PaymentResponse {
        public PaySafeCardPaymentResponseDetails Details { get; set; }
    }

    public class PaySafeCardPaymentResponseDetails {
        public string CustomerReference { get; set; }
    }
}