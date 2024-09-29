namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record OrderPaymentParameters {
        public string? CustomerId { get; set; }
        /// <summary>
        /// See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public string? SequenceType { get; set; }
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Adding an application fee allows you to charge the merchant for the payment and transfer this to your own account.
        /// </summary>
        public ApplicationFee? ApplicationFee { get; set; }
    }
}
