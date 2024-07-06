using System;

namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record BancontactPaymentResponse : PaymentResponse {
        public required BancontactPaymentResponseDetails? Details { get; set; }
    }

    public record BancontactPaymentResponseDetails {
        /// <summary>
        /// Only available if the payment is completed - The last four digits of the card number.
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
        /// Only available if the payment is completed - Unique alphanumeric representation of card, usable for
        /// identifying returning customers. This field is deprecated as of November 28th, 2019. The fingerprint
        /// is now unique per transaction what makes it not usefull anymore for identifying returning customers.
        /// Use the consumerAccount field instead.
        /// </summary>
        [Obsolete(@"This field is deprecated as of November 28th, 2019. The fingerprint is now unique per
transaction what makes it not usefull anymore for identifying returning customers. Use the consumerAccount field instead.")]
        public string? CardFingerprint { get; set; }

        /// <summary>
        /// Only available if requested during payment creation - The QR code that can be scanned by the mobile
        /// Bancontact application. This enables the desktop to mobile feature.
        /// </summary>
        public QrCode? QrCode { get; set; }

        /// <summary>
        /// Only available if the payment is completed – The consumer’s bank account. This may be an IBAN, or it may be a domestic account number.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment is completed – The consumer’s name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment is completed – The consumer’s bank’s BIC / SWIFT code.
        /// </summary>
        public string? ConsumerBic { get; set; }

        /// <summary>
        /// The reason why the payment did not succeed. Only available when there's a reason known.
        /// </summary>
        public string? FailureReason { get; set; }
    }
}
