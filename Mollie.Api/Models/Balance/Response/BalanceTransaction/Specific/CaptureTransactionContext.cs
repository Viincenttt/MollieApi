namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class CaptureTransactionContext : BaseTransactionContext {
        public string PaymentId { get; set; }
        public string CaptureId { get; set; }
    }
}