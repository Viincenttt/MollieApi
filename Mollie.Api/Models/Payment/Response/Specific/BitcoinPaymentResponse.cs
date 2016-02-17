namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BitcoinPaymentResponse : PaymentResponse {
        public BitcoinPaymentResponseDetails Details { get; set; }
    }

    public class BitcoinPaymentResponseDetails {
        public string BitcoinAddress { get; set; }
        public string BitcoinAmount { get; set; }
        public string BitcoinRate { get; set; }
        public string BitcoinUri { get; set; }
    }
}