namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class CaptureBalanceTransaction : BalanceTransaction {
        public CaptureTransactionContext Context { get; set; }
    }
    
    public class CaptureTransactionContext {
        public string PaymentId { get; set; }
        public string CaptureId { get; set; }
    }
}