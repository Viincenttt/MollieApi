namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class RefundBalanceTransaction : BalanceTransaction {
        public required RefundTransactionContext Context { get; init; }
    }
    
    public class RefundTransactionContext {
        public required string PaymentId { get; init; }
        public required string RefundId { get; init; }
    }
}