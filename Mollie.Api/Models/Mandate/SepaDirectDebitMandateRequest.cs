namespace Mollie.Api.Models.Mandate
{
    public class SepaDirectDebitMandateRequest : MandateRequest
    {
        public SepaDirectDebitMandateRequest() {
            this.Method = Payment.PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Required for `directdebit` mandates - Consumer's IBAN account
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Optional - The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string ConsumerBic { get; set; }

    }
}
