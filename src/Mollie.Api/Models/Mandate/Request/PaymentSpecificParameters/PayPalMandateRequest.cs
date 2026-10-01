namespace Mollie.Api.Models.Mandate.Request.PaymentSpecificParameters
{
    public record PayPalMandateRequest : MandateRequest
    {
        public PayPalMandateRequest() {
            Method = Payment.PaymentMethod.PayPal;
        }

        /// <summary>
        /// Required For Paypal - The consumer's email address.
        /// </summary>
        public required string ConsumerEmail { get; set; }

        /// <summary>
        /// The billing agreement ID given by PayPal. Either this field or PayPalVaultId must be provided, but not both.
        /// </summary>
        public string? PaypalBillingAgreementId { get; set; }

        /// <summary>
        /// The vault ID given by PayPal. Either this field or PaypalBillingAgreementId must be provided, but not both.
        /// </summary>
        public string? PayPalVaultId { get; set; }
    }
}
