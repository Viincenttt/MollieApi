namespace Mollie.Api.Models.Mandate.Request.PaymentSpecificParameters
{
    public record SepaDirectDebitMandateRequest : MandateRequest
    {
        public SepaDirectDebitMandateRequest() {
            Method = Payment.PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Required for `directdebit` mandates - Consumer's IBAN account
        /// </summary>
        public required string ConsumerAccount { get; set; }

        /// <summary>
        /// Optional - The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string? ConsumerBic { get; set; }

    }
}
