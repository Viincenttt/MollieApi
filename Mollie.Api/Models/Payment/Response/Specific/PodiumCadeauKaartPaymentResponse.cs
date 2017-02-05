namespace Mollie.Api.Models.Payment.Response.Specific {
    public class PodiumCadeauKaartPaymentResponse : PaymentResponse {
        public PodiumCadeauKaartPaymentResponseDetails Details { get; set; }
    }
    
    public class PodiumCadeauKaartPaymentResponseDetails {
        /// <summary>
        /// The last four digits of the card number.
        /// </summary>
        public string CardNumber { get; set; }
    }
}
