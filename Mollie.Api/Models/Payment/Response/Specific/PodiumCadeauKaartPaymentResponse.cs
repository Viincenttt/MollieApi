namespace Mollie.Api.Models.Payment.Response.Specific {
    public class PodiumCadeauKaartPaymentResponse : PaymentResponse {
        public PodiumCadeauKaartPaymentResponseDetails Details { get; set; }
    }
    
    public class PodiumCadeauKaartPaymentResponseDetails {
        public string CardNumber { get; set; }
    }
}
