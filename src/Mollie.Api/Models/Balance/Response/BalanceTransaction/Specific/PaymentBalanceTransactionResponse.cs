namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class PaymentBalanceTransactionResponse : BalanceTransactionResponse {
        public required PaymentTransactionContext Context { get; set; }
    }

    public class PaymentTransactionContext {
        public required string PaymentId { get; set; }

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string? PaymentDescription { get; set; }

        /// <summary>
        /// The organization that received the commission. Only available for the payment-commission and
        /// capture-commission transaction types.
        /// </summary>
        public string? OrganizationId { get; set; }

        /// <summary>
        /// The organization that paid the application fee. Only available for the application-fee transaction type.
        /// </summary>
        public string? PayingOwner { get; set; }

        /// <summary>
        /// The organization that owns the payment. Only available for the split-payment transaction type.
        /// </summary>
        public string? PaymentOwner { get; set; }
    }
}
