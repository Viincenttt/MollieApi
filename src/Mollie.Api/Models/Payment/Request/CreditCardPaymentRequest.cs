namespace Mollie.Api.Models.Payment.Request {
    public record CreditCardPaymentRequest : PaymentRequest {
        public CreditCardPaymentRequest() {
            this.Method = PaymentMethod.CreditCard;
        }

        /// <summary>
        /// The card holder�s address details. We advise to provide these details to improve the credit card 
        /// fraud protection, and thus improve conversion.
        /// </summary>
        public AddressObject? BillingAddress { get; set; }

        /// <summary>
        /// The shipping address details. We advise to provide these details to improve the credit card fraud 
        /// protection, and thus improve conversion.
        /// </summary>
        public AddressObject? ShippingAddress { get; set; }

        /// <summary>
        /// The card token you get from Mollie Components. The token contains the card information
        /// (such as card holder, card number and expiry date) needed to complete the payment.
        /// </summary>
        public string? CardToken { get; set; }
    }
}