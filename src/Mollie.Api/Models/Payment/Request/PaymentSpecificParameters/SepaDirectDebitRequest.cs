using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record SepaDirectDebitRequest : PaymentRequest {
        public SepaDirectDebitRequest() {
            Method = PaymentMethod.DirectDebit;
        }

        [SetsRequiredMembers]
        public SepaDirectDebitRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Optional - Beneficiary name of the account holder.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Optional - IBAN of the account holder.
        /// </summary>
        public string? ConsumerAccount { get; set; }
    }
}
