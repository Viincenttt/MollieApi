namespace Mollie.Api.Models.Payment.Request {
    public class CreditCardPaymentRequest : PaymentRequest {
        /// <summary>
        /// Optional - The card holder's address. We advise to provide these details to improve the credit card fraude protection, and thus improve conversion.
        /// </summary>
        public string BillingAddress { get; set; }
        /// <summary>
        /// Optional - The card holder's city.
        /// </summary>
        public string BillingCity { get; set; }
        /// <summary>
        /// Optional - The card holder's region.
        /// </summary>
        public string BillingRegion { get; set; }
        /// <summary>
        /// Optional - The card holder's postal code.
        /// </summary>
        public string BillingPostal { get; set; }
        /// <summary>
        /// Optional - The card holder's country in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string BillingCountry { get; set; }
        /// <summary>
        /// Optional - The shipping address. We advise to provide these details to improve the credit card fraude protection, and thus improve conversion.
        /// </summary>
        public string ShippingAddress { get; set; }
        /// <summary>
        /// Optional - The city of the shipping address.
        /// </summary>
        public string ShippingCity { get; set; }
        /// <summary>
        /// Optional - The region of the shipping address.
        /// </summary>
        public string ShippingRegion { get; set; }
        /// <summary>
        /// Optional - The postal code of the shipping address.
        /// </summary>
        public string ShippingPostal { get; set; }
        /// <summary>
        /// Optional - The country of the shipping address, in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string ShippingCountry { get; set; }

        public CreditCardPaymentRequest() {
            this.Method = PaymentMethod.CreditCard;
        }
    }
}