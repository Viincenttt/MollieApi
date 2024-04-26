namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record PaymentSpecificParameters {
        public string? CustomerId { get; set; }
        /// <summary>
        /// See the Mollie.Api.Models.Payment.SequenceType class for a full list of known values.
        /// </summary>
        public string? SequenceType { get; set; }
        public string? WebhookUrl { get; set; }
    }
}
