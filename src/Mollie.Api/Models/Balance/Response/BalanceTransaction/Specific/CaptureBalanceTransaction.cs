namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class CaptureBalanceTransaction : BalanceTransaction {
        public required CaptureTransactionContext Context { get; set; }
    }

    public class CaptureTransactionContext {
        public required string PaymentId { get; set; }
        public required string CaptureId { get; set; }
    }
}
