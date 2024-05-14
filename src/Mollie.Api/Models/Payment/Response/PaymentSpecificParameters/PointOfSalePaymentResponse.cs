namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record PointOfSalePaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public required PointOfSalePaymentResponseDetails Details { get; set; }
    }

    public record PointOfSalePaymentResponseDetails {
        /// <summary>
        /// The identifier referring to the terminal this payment was created for. For example, term_utGtYu756h.
        /// </summary>
        public required string TerminalId { get; set; }

        /// <summary>
        /// Only available if the payment has been completed - The last four digits of the card number.
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
        /// Only available if the payment has been completed - Unique alphanumeric representation of card, usable for
        /// identifying returning customers.
        /// </summary>
        public string? CardFingerprint { get; set; }

        /// <summary>
        /// Only available if the payment has been completed and if the data is available - The card’s target audience.
        ///
        /// Check the Mollie.Api.Models.Payment.Response.CreditCardAudience class for a full list of known values.
        /// </summary>
        public string? CardAudience { get; set; }

        /// <summary>
        /// Only available if the payment has been completed - The card’s label. Note that not all labels can be
        /// processed through Mollie.
        ///
        ///  Check the Mollie.Api.Models.Payment.Response.CreditCardLabel class for a full list of known values.
        /// </summary>
        public string? CardLabel { get; set; }

        /// <summary>
        /// Only available if the payment has been completed - The ISO 3166-1 alpha-2 country code of the country
        /// the card was issued in. For example: BE.
        /// </summary>
        public string? CardCountryCode { get; set; }
    }
}
