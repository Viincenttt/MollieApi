using System.Diagnostics.CodeAnalysis;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record GiftcardPaymentRequest : PaymentRequest {
        public GiftcardPaymentRequest() {
            Method = PaymentMethod.GiftCard;
        }

        [SetsRequiredMembers]
        public GiftcardPaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.GiftCard;
        }

        /// <summary>
        /// The card number on the gift card.
        /// </summary>
        public string? VoucherNumber { get; set; }

        /// <summary>
        /// The PIN code on the gift card. Only required if there is a PIN code printed on the gift card.
        /// </summary>
        public string? VoucherPin { get; set; }
    }
}
