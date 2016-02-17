namespace Mollie.Api.Models.Payment.Response {
    public class MisterCashPaymentResponse : PaymentResponse {
        public MisterCashPaymentResponseDetails Details { get; set; }
    }

    public class MisterCashPaymentResponseDetails {
        public string CardNumber { get; set; }
    }
}