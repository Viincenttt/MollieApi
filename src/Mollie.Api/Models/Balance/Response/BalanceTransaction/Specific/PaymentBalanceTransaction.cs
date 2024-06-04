namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class PaymentBalanceTransaction : BalanceTransaction {
        public required PaymentTransactionContext Context { get; set; }
    }

    public class PaymentTransactionContext {
        public required string PaymentId { get; set; }
    }
}
