namespace Mollie.Api.Models.Payment.Response {
    public class CreditCardPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with credit card details.
        /// </summary>
        public CreditCardPaymentResponseDetails Details { get; set; }
    }

    public class CreditCardPaymentResponseDetails {
        /// <summary>
        /// The card holder's name.
        /// </summary>
        public string CardHolder { get; set; }

        /// <summary>
        /// The last four digits of the card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Only available if the payment has been completed - Unique alphanumeric representation of card, usable for identifying 
        /// returning customers.
        /// </summary>
        public string CardFingerprint { get; set; }

        /// <summary>
        /// Not always available. – The card's target audience. See the Mollie.Api.Models.Payment.Response.CreditCardAudience 
        /// class for a full list of known values
        /// </summary>
        public string CardAudience { get; set; }

        /// <summary>
        /// The card's label. Note that not all labels can be acquired through Mollie. See the
        /// Mollie.Api.Models.Payment.Response.CreditCardLabel class for a full list of known values
        /// </summary>
        public string CardLabel { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 country code of the country the card was issued in. For example: BE.
        /// </summary>
        public string CardCountryCode { get; set; }

        /// <summary>
        /// Only available if the payment succeeded. – The payment's security type. See the 
        /// Mollie.Api.Models.Payment.Response.CreditCardSecurity class for a full list of known values
        /// </summary>
        public string CardSecurity { get; set; }

        /// <summary>
        /// Only available if the payment succeeded. – The fee region for the payment. See your credit card addendum for
        /// details. intra-eu for consumer cards from the EU, and other for all other cards. See the 
        /// Mollie.Api.Models.Payment.Response.CreditCardFeeRegion class for a full list of known values
        /// </summary>
        public string FeeRegion { get; set; }

        /// <summary>
        /// Only available for failed payments. Contains a failure reason code. See the 
        /// Mollie.Api.Models.Payment.Response.CreditCardFailureReason class for a full list of known values
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// A localized message that can be shown to your customer, depending on the failureReason
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// The wallet used when creating the payment.
        /// </summary>
        public string Wallet { get; set; }
    }

    /// <summary>
    /// The card's target audience.
    /// </summary>
    public static class CreditCardAudience {
        public const string Consumer = "consumer";
        public const string Business = "business";
    }

    /// <summary>
    /// Only available if the payment has been completed – The type of security used during payment processing.
    /// </summary>
    public static class CreditCardSecurity {
        public const string Normal = "normal";
        public const string Secure3D = "3dsecure";
    }

    /// <summary>
    /// Only available if the payment has been completed – The fee region for the payment: intra-eu for consumer cards from the EU, and 
    /// other for all other cards.
    /// </summary>
    public static class CreditCardFeeRegion {
        public const string IntraEu = "intra-eu";
        public const string AmericanExpress = "american-express";
        public const string CarteBancaire = "carte-bancaire";
        public const string Maestro = "maestro";
    }

    /// <summary>
    /// Only available for failed payments. Contains a failure reason code.
    /// </summary>
    public static class CreditCardFailureReason {
        public const string InvalidCardNumber = "invalid_card_number";
        public const string InvalidCvv = "invalid_cvv";
        public const string InvalidCardHolderName = "invalid_card_holder_name";
        public const string CardExpired = "CardExpired";
        public const string InvalidCardType = "invalid_card_type";
        public const string RefusedByIssuer = "refused_by_issuer";
        public const string InsufficientFunds = "insufficient_funds";
        public const string InactiveCard = "inactive_card";
        public const string UnknownReason = "unknown_reason";
        public const string PossibleFraud = "possible_fraud";
        public const string AuthenticationFailed = "authentication_failed";
    }

    /// <summary>
    /// The card's label. Note that not all labels can be acquired through Mollie.
    /// </summary>    
    public static class CreditCardLabel {
        public const string AmericanExpress = "American Express";
        public const string CartaSi = "Carta si";
        public const string CarteBleue = "Carte Bleue";
        public const string Dankort = nameof(Dankort);
        public const string DinersClub = "Diners Club";
        public const string Discover = nameof(Discover);
        public const string Jcb = "JCB";
        public const string Laser = nameof(Laser);
        public const string Maestro = nameof(Maestro);
        public const string Mastercard = nameof(Mastercard);
        public const string Unionpay = nameof(Unionpay);
        public const string Visa = nameof(Visa);
    }
}