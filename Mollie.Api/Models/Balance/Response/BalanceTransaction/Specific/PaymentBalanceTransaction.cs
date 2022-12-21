namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class PaymentBalanceTransaction : BalanceTransaction {
        public PaymentTransactionContext Context { get; set; }
    }
    
    public class PaymentTransactionContext {
        public string PaymentId { get; set; }
    }
}