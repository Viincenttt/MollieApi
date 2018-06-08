namespace Mollie.Api.Models.Refund {
    public class RefundRequest {
        /// <summary>
        /// The amount to refund. For some payments, it can be up to €25.00 more than the original transaction amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Optional – The description of the refund you are creating. This will be shown to the consumer on their card or bank
        /// statement when possible. Max 140 characters.
        /// </summary>
        public string Description { get; set; }
    }
}