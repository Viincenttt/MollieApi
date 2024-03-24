namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class CaptureBalanceTransaction : BalanceTransaction {
        public required CaptureTransactionContext Context { get; init; }
    }
    
    public class CaptureTransactionContext {
        public required string PaymentId { get; init; }
        public required string CaptureId { get; init; }
    }
}