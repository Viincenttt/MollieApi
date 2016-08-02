namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BelfiusPaymentResponse : PaymentResponse {
        public BelfiusPaymentResponseDetails Details { get; set; }
    }

    public class BelfiusPaymentResponseDetails {
        public string ConsumerName { get; set; }
        public string ConsumerAccount { get; set; }
        public string ConsumerBic { get; set; }
    }
}
