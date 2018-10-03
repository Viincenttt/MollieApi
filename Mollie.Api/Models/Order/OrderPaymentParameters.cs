using System;
using Mollie.Api.Models.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Order {
    public class OrderPaymentParameters {
        /// <summary>
        /// IBAN of the account holder. Only available if one-off payments are enabled on your account.
        /// Will pre-fill the IBAN in the checkout screen if present.
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// The ID of the Customer for whom the payment is being created. This is used for recurring payments and single click
        /// payments.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Used for consumer identification. For example, you could use the consumer’s IP address.
        /// </summary>
        public string CustomerReference { get; set; }

        /// <summary>
        /// The date the payment should expire, in YYYY-MM-DD format. Please note: the minimum date is tomorrow and the maximum
        /// date is 100 days after tomorrow.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// The gift card brand to use for the payment. These issuers can be retrieved by using the issuers include in the
        /// Methods API.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// When creating recurring payments, the ID of a specific Mandate may be supplied to indicate which of the consumer’s
        /// accounts should be credited.
        /// </summary>
        public string MandateId { get; set; }

        /// <summary>
        /// Indicate which type of payment this is in a recurring sequence. If set to first, a first payment is created for the 
        /// customer, allowing the customer to agree to automatic recurring charges taking place on their account in the future. 
        /// If set to recurring, the customer’s card is charged automatically. Defaults to oneoff, which is a regular non-recurring 
        /// payment(see also: Recurring).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SequenceType? SequenceType { get; set; }

        /// <summary>
        /// The card number on the gift card.
        /// </summary>
        public string VoucherNumber { get; set; }

        /// <summary>
        /// The PIN code on the gift card. Only required if there is a PIN code printed on the gift card.
        /// </summary>
        public string VoucherPin { get; set; }
    }
}