namespace Mollie.Api.Models.Payment.Response.Specific {
    public class SepaDirectDebitResponse : PaymentResponse {
        public SepaDirectDebitResponseDetails Details { get; set; }
    }

    public class SepaDirectDebitResponseDetails {
        /// <summary>
        /// Transfer reference used by Mollie to identify this payment.
        /// </summary>
        public string TransferReference { get; set; }

        /// <summary>
        /// The creditor identifier indicates who is authorized to execute the payment. In this case, it is a reference to Mollie.
        /// </summary>
        public string CreditorIdentifier { get; set; }

        /// <summary>
        /// Optional – The consumer's name.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Optional – The consumer's IBAN.
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Optional – The consumer's bank's BIC.
        /// </summary>
        public string ConsumerBic { get; set; }

        /// <summary>
        /// Only available if the payment has been verified – Date the payment has been signed by the consumer, in ISO 8601 format.
        /// </summary>
        public string SignatureDate { get; set; }

        /// <summary>
        /// Only available if the payment has failed – The official reason why this payment has failed. A detailed description of each reason is available on the website of 
        /// the European Payments Council.
        /// </summary>
        public string BankReasonCode { get; set; }

        /// <summary>
        /// Only available if the payment has failed – A textual desciption of the failure reason.
        /// </summary>
        public string BankReason { get; set; }

        /// <summary>
        /// Only available for batch transactions – The original end-to-end identifier that you've specified in your batch.
        /// </summary>
        public string EndToEndIdentifier { get; set; }

        /// <summary>
        /// Only available for batch transactions – The original mandate reference that you've specified in your batch.
        /// </summary>
        public string MandateReference { get; set; }

        /// <summary>
        /// Only available for batch transactions – The original batch reference that you've specified in your batch.
        /// </summary>
        public string BatchReference { get; set; }

        /// <summary>
        /// Only available for batch transactions – The original file reference that you've specified in your batch.
        /// </summary>
        public string FileReference { get; set; }
    }
}
