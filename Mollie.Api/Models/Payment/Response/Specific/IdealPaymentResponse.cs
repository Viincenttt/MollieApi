namespace Mollie.Api.Models.Payment.Response {
    public class IdealPaymentResponse : PaymentResponse {
        public IdealPaymentResponseDetails Details { get; set; }
    }

    public class IdealPaymentResponseDetails {
        public string ConsumerName { get; set; }
        public string ConsumerAccount { get; set; }
        public string ConsumerBic { get; set; }
    }
}