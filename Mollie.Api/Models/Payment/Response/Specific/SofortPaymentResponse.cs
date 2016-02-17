namespace Mollie.Api.Models.Payment.Response {
    public class SofortPaymentResponse : PaymentResponse {
        public SofortPaymentResponseDetails Details { get; set; }
    }

    public class SofortPaymentResponseDetails {
        public string ConsumerName { get; set; }
        public string ConsumerAccount { get; set; }
        public string ConsumerBic { get; set; }
    }
}