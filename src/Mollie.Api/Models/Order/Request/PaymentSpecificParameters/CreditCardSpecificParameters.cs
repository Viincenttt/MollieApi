namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record CreditCardSpecificParameters : OrderPaymentParameters {
        /// <summary>
        /// The card token you get from Mollie Components. The token contains the card information
        /// (such as card holder, card number and expiry date) needed to complete the payment.
        /// </summary>
        public string? CardToken { get; set; }
    }
}