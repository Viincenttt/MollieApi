namespace Mollie.Api.Models.Order.Request {
    public record OrderUpdateRequest {
        /// <summary>
        /// The billing person and address for the order. See Order address details for the
        /// exact fields needed.
        /// </summary>
        public OrderAddressDetails? BillingAddress { get; set; }

        /// <summary>
        /// The shipping address for the order. See Order address details for the exact
        /// fields needed.
        /// </summary>
        public OrderAddressDetails? ShippingAddress { get; set; }

        /// <summary>
        /// The order number. For example, 16738. We recommend that each order should have a unique order number.
        /// </summary>
        public string? OrderNumber { get; set; }

        /// <summary>
        /// The URL your customer will be redirected to after the payment process. Updating this field is only possible
        /// when the payment is not yet finalized.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// The URL your consumer will be redirected to when the consumer explicitly cancels the order. If this URL
        /// is not provided, the consumer will be redirected to the redirectUrl instead — see above. Updating this
        /// field is only possible when the payment is not yet finalized.
        /// </summary>
        public string? CancelUrl { get; set; }

        /// <summary>
        /// Set the webhook URL, where we will send order status changes to. The webhookUrl must be reachable from
        /// Mollie’s point of view, so you cannot use localhost. If you want to use webhook during development on
        /// localhost, you should use a tool like ngrok to have the webhooks delivered to your local machine.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
