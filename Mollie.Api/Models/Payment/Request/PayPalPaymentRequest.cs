namespace Mollie.Api.Models.Payment.Request {
    public class PayPalPaymentRequest : PaymentRequest {
        public PayPalPaymentRequest() {
            this.Method = PaymentMethod.PayPal;
        }

        public AddressObject ShippingAddress { get; set; }
    }
}