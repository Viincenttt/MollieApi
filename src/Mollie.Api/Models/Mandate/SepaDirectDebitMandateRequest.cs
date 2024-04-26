using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Mandate
{
    public record SepaDirectDebitMandateRequest : MandateRequest
    {
        public SepaDirectDebitMandateRequest() {
            Method = Payment.PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Required for `directdebit` mandates - Consumer's IBAN account
        /// </summary>
        public required string ConsumerAccount { get; init; }

        /// <summary>
        /// Optional - The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string? ConsumerBic { get; set; }

    }
}
