namespace Mollie.Api.Models.Payment.Response {
    public class PaymentResponseLinks {
        /// <summary>
        /// The API resource URL of the payment itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The URL your customer should visit to make the payment. This is where you should redirect the consumer to.
        /// </summary>
        public UrlObject Checkout { get; set; }

        /// <summary>
        /// The API resource URL of the refunds that belong to this payment.
        /// </summary>
        public UrlObject Refunds { get; set; }

        /// <summary>
        /// The API resource URL of the chargebacks that belong to this payment.
        /// </summary>
        public UrlObject Chargebacks { get; set; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObject Settlement { get; set; }

        /// <summary>
        /// The URL to the payment retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }

        /// <summary>
        /// The API resource URL of the mandate linked to this payment. Not present if a one-off payment.
        /// </summary>
        public UrlObject Mandate { get; set; }

        /// <summary>
        /// The API resource URL of the subscription this payment is part of. Not present if not a subscription payment.
        /// </summary>
        public UrlObject Subscription { get; set; }

        /// <summary>
        /// The API resource URL of the customer this payment belongs to. Not present if not linked to a customer.
        /// </summary>
        public UrlObject Customer { get; set; }
    }
}