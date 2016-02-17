namespace Mollie.Api.Models.Payment.Response {
    public class CreditCardPaymentResponse : PaymentResponse {
        public CreditCardPaymentResponseDetails Details { get; set; }
    }

    public class CreditCardPaymentResponseDetails {
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string CardSecurity { get; set; }
    }
}