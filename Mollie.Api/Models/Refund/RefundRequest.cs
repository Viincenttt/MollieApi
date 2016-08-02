namespace Mollie.Api.Models.Refund {
    public class RefundRequest {
        /// <summary>
        /// Optional – The amount to refund. When ommitted, the full amount is refunded.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank statement when possible. Max 140 characters.
        /// </summary>
        public string Message { get; set; }
    }
}
