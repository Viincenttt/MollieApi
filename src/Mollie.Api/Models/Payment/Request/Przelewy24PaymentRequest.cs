namespace Mollie.Api.Models.Payment.Request
{
    public record Przelewy24PaymentRequest : PaymentRequest
    {
        public Przelewy24PaymentRequest()
        {
            this.Method = PaymentMethod.Przelewy24;
        }

        /// <summary>
        /// Consumer�s email address, this is required for Przelewy24 payments.
        /// </summary>
        public string? BillingEmail { get; set; }
    }
}