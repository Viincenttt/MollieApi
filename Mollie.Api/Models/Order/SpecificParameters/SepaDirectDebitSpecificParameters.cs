namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class SepaDirectDebitSpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// Optional - Beneficiary name of the account holder.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Optional - IBAN of the account holder.
        /// </summary>
        public string ConsumerAccount { get; set; }
    }
}
