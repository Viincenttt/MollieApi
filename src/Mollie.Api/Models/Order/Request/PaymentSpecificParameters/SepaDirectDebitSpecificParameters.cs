namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record SepaDirectDebitSpecificParameters : OrderPaymentParameters {
        /// <summary>
        /// Optional - IBAN of the account holder.
        /// </summary>
        public string? ConsumerAccount { get; set; }
    }
}
