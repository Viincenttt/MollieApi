namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BitcoinPaymentResponse : PaymentResponse {
        public BitcoinPaymentResponseDetails Details { get; set; }
    }

    public class BitcoinPaymentResponseDetails {
        /// <summary>
        /// The bitcoin address the bitcoins were transferred to.
        /// </summary>
        public string BitcoinAddress { get; set; }

        /// <summary>
        /// The amount transferred in BTC.
        /// </summary>
        public string BitcoinAmount { get; set; }

        /// <summary>
        /// The BTC to EUR exchange rate applied to the payment.
        /// </summary>
        public string BitcoinRate { get; set; }

        /// <summary>
        /// A URI that is understood by Bitcoin wallet clients and will cause such clients to prepare the transaction.
        /// </summary>
        public string BitcoinUri { get; set; }

        /// <summary>
        /// Only available when explicitly included. – A QR code that can be scanned by Bitcoin wallet clients and will cause such clients to prepare the transaction.
        /// </summary>
        public QrCode QrCode { get; set; }
    }
}