namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class ChargebackBalanceTransactionResponse : BalanceTransactionResponse {
        public required ChargebackTransactionContext Context { get; set; }
    }

    public class ChargebackTransactionContext {
        public required string PaymentId { get; set; }
        public required string ChargebackId { get; set; }

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string? PaymentDescription { get; set; }

        /// <summary>
        /// The description of the chargeback.
        /// </summary>
        public string? ChargebackDescription { get; set; }
    }
}
