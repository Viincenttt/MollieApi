namespace Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific {
    public class CaptureBalanceTransactionResponse : BalanceTransactionResponse {
        public required CaptureTransactionContext Context { get; set; }
    }

    public class CaptureTransactionContext {
        public required string PaymentId { get; set; }
        public required string CaptureId { get; set; }

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string? PaymentDescription { get; set; }

        /// <summary>
        /// The description of the capture.
        /// </summary>
        public string? CaptureDescription { get; set; }
    }
}
