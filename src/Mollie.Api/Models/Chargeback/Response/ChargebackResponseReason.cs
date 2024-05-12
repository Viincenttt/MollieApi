namespace Mollie.Api.Models.Chargeback.Response {
    public record ChargebackResponseReason {
        /// <summary>
        /// The reason for the chargeback, these are documented here on Mollie's website https://help.mollie.com/hc/en-us/articles/115000309865-Why-did-my-direct-debit-payment-fail-
        /// </summary>
        public required string Code { get; init; }
        /// <summary>
        /// an accompanying note to the code
        /// </summary>
        public required string Description { get; init; }
    }
}
