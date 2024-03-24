namespace Mollie.Api.Models.Payment.Request {
    public record PayPalPaymentRequest : PaymentRequest {
        public PayPalPaymentRequest() {
            this.Method = PaymentMethod.PayPal;
        }

        /// <summary>
        /// The shipping address details. We advise to provide these details to improve PayPal�s fraud protection, 
        /// and thus improve conversion.
        /// </summary>
        public AddressObject? ShippingAddress { get; set; }

        /// <summary>
        /// The unique ID you have used for the PayPal fraud library. You should include this if you use PayPal 
        /// for an on-demand payment. The maximum character length is 32.
        /// Please refer to the Recurring payments guide for more information on how to implement the fraud library.
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// Indicate if you�re about to deliver digital goods, like for example a license. Setting this parameter can
        /// have consequences for your Seller Protection by PayPal. Please see PayPal�s help article about Seller
        /// Protection for more information.
        /// </summary>
        public bool? DigitalGoods { get; set; }
    }
}