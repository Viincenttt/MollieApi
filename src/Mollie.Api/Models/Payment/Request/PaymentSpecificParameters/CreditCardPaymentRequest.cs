using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record CreditCardPaymentRequest : PaymentRequest {
        public CreditCardPaymentRequest() {
            Method = PaymentMethod.CreditCard;
        }

        [SetsRequiredMembers]
        public CreditCardPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.CreditCard;
        }

        /// <summary>
        /// The card token you get from Mollie Components. The token contains the card information
        /// (such as card holder, card number and expiry date) needed to complete the payment.
        /// </summary>
        public string? CardToken { get; set; }
    }
}
