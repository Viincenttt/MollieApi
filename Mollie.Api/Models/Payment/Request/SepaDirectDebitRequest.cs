namespace Mollie.Api.Models.Payment.Request {
    public class SepaDirectDebitRequest : PaymentRequest {
        /// <summary>
        /// Optional - Beneficiary name of the account holder.
        /// </summary>
        public string ConsumerName { get; set; }
        /// <summary>
        /// Optional - IBAN of the account holder.
        /// </summary>
        public string ConsumerAccount { get; set; }

        public SepaDirectDebitRequest() {
            this.Method = PaymentMethod.DirectDebit;
        }
    }
}