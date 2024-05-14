namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record GiftcardPaymentRequest : PaymentRequest {
        public GiftcardPaymentRequest() {
            Method = PaymentMethod.GiftCard;
        }

        /// <summary>
        /// The gift card brand to use for the payment. These issuers are not dynamically available through the Issuers API,
        /// but can be retrieved by using the issuers include in the Methods API. If you need a brand not in the list, contact
        /// our support department. If only one issuer is activated on your account, you can omit this parameter.
        /// </summary>
        public string? Issuer { get; set; }

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
