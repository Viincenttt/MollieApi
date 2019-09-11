namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class Przelewy24SpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// Consumer’s email address, this is required for Przelewy24 payments.
        /// </summary>
        public string BillingEmail { get; set; }
    }
}
