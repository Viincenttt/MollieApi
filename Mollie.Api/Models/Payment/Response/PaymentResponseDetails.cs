namespace Mollie.Api.Models.Payment.Response {
    public class PaymentResponseDetails {
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string CardSecurity { get; set; }

        public string BitcoinAddress { get; set; }
        public string BitcoinAmount { get; set; }
        public string BitcoinRate { get; set; }
        public string BitcoinUri { get; set; }

        public string ConsumerName { get; set; }
        public string ConsumerAccount { get; set; }
        public string ConsumerBic { get; set; }

        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankBic { get; set; }
        public string TransferReference { get; set; }

        public string PayPalReference { get; set; }

        public string CustomerReference { get; set; }
    }
}
