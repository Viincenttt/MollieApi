using System.Collections.Generic;

namespace Mollie.Api.Models.Payment.Response.Specific {
    public class GiftcardPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public required GiftcardPaymentResponseDetails Details { get; init; }
    }

    public class GiftcardPaymentResponseDetails {
        /// <summary>
        /// The voucher number, with the last four digits masked. When multiple gift cards are used, this is the first voucher 
        /// number. Example: 606436353088147****.
        /// </summary>
        public string VoucherNumber { get; set; }

        /// <summary>
        /// A list of details of all giftcards that are used for this payment. Each object will contain the following properties.
        /// </summary>
        public List<Giftcard> Giftcards { get; set; }

        /// <summary>
        /// Only available if another payment method was used to pay the remainder amount – The amount that was paid with 
        /// another payment method for the remainder amount.
        /// </summary>
        public Amount? RemainderAmount { get; set; }

        /// <summary>
        /// Only available if another payment method was used to pay the remainder amount – The payment method that was used to 
        /// pay the remainder amount.
        /// </summary>
        public string? RemainderMethod { get; set; }
    }

    public class Giftcard {
        /// <summary>
        /// The ID of the gift card brand that was used during the payment.
        /// </summary>
        public required string Issuer { get; init; }

        /// <summary>
        /// The amount in EUR that was paid with this gift card.
        /// </summary>
        public required Amount Amount { get; init; }

        /// <summary>
        /// The voucher number, with the last four digits masked. Example: 606436353088147****
        /// </summary>
        public required string VoucherNumber { get; init; }
    }
}