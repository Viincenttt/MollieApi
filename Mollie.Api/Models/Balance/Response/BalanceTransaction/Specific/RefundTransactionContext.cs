namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class RefundTransactionContext : BaseTransactionContext {
        public string PaymentId { get; set; }
        public string RefundId { get; set; }
    }
}