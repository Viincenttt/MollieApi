namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class RefundBalanceTransactionResponse : BalanceTransactionResponse {
        public required RefundTransactionContext Context { get; set; }
    }

    public class RefundTransactionContext {
        public required string PaymentId { get; set; }
        public required string RefundId { get; set; }

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string? PaymentDescription { get; set; }

        /// <summary>
        /// The description of the refund.
        /// </summary>
        public string? RefundDescription { get; set; }
    }
}
