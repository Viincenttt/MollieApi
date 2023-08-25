namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class RefundBalanceTransaction : BalanceTransaction {
        public RefundTransactionContext Context { get; set; }
    }
    
    public class RefundTransactionContext {
        public string PaymentId { get; set; }
        public string RefundId { get; set; }
    }
}