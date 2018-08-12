namespace Mollie.Api.Models.Payment.Request {
    public class CreditCardPaymentRequest : PaymentRequest {
        public CreditCardPaymentRequest() {
            this.Method = PaymentMethod.CreditCard;
        }

        /// <summary>
        /// The card holder’s address details. We advise to provide these details to improve the credit card 
        /// fraud protection, and thus improve conversion.
        /// </summary>
        public AddressObject BillingAddress { get; set; }

        /// <summary>
        /// The shipping address details. We advise to provide these details to improve the credit card fraud 
        /// protection, and thus improve conversion.
        /// </summary>
        public AddressObject ShippingAddress { get; set; }
    }
}