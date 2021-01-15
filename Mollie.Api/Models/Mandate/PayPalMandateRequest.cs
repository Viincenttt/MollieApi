namespace Mollie.Api.Models.Mandate
{
    public class PayPalMandateRequest : MandateRequest
    {
        public PayPalMandateRequest() {
            this.Method = Payment.PaymentMethod.PayPal;
        }

        /// <summary>
        /// Required For Paypal - The consumer's email address.
        /// </summary>
        public string ConsumerEmail { get; set; }

        /// <summary>
        /// Required for `paypal` mandates - The billing agreement ID given by PayPal.
        /// </summary>
        public string PaypalBillingAgreementId { get; set; }
    }
}
