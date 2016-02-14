namespace Mollie.Api.Models.Payment.Request {
    public class PayPalPaymentRequest : PaymentRequest {
        /// <summary>
        /// Optional - The shipping address. We advise to provide these details to improve PayPal's fraude protection, and thus improve conversion. 
        /// The maximum character length is 128.
        /// </summary>
        public string ShippingAddress { get; set; }
        /// <summary>
        /// Optional - The city of the shipping address. The maximum character length is 100.
        /// </summary>
        public string ShippingCity { get; set; }
        /// <summary>
        /// Optional - The region of the shipping address. The maximum character length is 100. This field is required if the shippingCountry is one 
        /// of the following countries:
        /// </summary>
        public string ShippingRegion { get; set; }
        /// <summary>
        /// Optional - The postal code of the shipping address. The maximum character length is 20.
        /// </summary>
        public string ShippingPostal { get; set; }
        /// <summary>
        /// Optional - The country of the shipping address, in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string ShippingCountry { get; set; }

        public PayPalPaymentRequest() {
            this.Method = PaymentMethod.PayPal;
        }
    }
}