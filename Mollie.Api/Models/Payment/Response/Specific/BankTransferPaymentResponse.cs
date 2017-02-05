namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BankTransferPaymentResponse : PaymentResponse {
        public BankTransferPaymentResponseDetails Details { get; set; }
    }

    public class BankTransferPaymentResponseDetails {
        /// <summary>
        /// The name of the bank the consumer should wire the amount to.
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// The IBAN the consumer should wire the amount to.
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// The BIC of the bank the consumer should wire the amount to.
        /// </summary>
        public string BankBic { get; set; }

        /// <summary>
        /// The reference the consumer should use when wiring the amount. Note you should not apply any formatting here; show it to the consumer as-is.
        /// </summary>
        public string TransferReference { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's name.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's bank account. This may be an IBAN, or it may be a domestic account number.
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string ConsumerBic { get; set; }

        /// <summary>
        /// Only available when explicitly included. – A QR code that can be scanned by Bitcoin wallet clients and will cause such clients to prepare the transaction.
        /// </summary>
        public QrCode QrCode { get; set; }
    }
}