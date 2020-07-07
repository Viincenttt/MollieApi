using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        /// Not always available. – The card's target audience.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreditCardAudience? CardAudience { get; set; }

        /// <summary>
        /// The card's label. Note that not all labels can be acquired through Mollie.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreditCardLabel? CardLabel { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 country code of the country the card was issued in. For example: BE.
        /// </summary>
        public string CardCountryCode { get; set; }

        /// <summary>
        /// Only available if the payment succeeded. – The payment's security type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreditCardSecurity? CardSecurity { get; set; }

        /// <summary>
        /// Only available if the payment succeeded. – The fee region for the payment. See your credit card addendum for
        /// details. intra-eu for consumer cards from the EU, and other for all other cards. Use the CreditCardFeeRegion
        /// static class for a list of known credit card fee regions.
        /// </summary>
        public string FeeRegion { get; set; }

        /// <summary>
        /// Only available for failed payments. Contains a failure reason code. See the 
        /// Mollie.Api.Models.Payment.Response.CreditCardFailureReason class for a full list of known failure reasons
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// A localized message that can be shown to your customer, depending on the failureReason
        /// </summary>
        public string FailureMessage { get; set; }
    }

    /// <summary>
    /// The card's target audience.
    /// </summary>
    public enum CreditCardAudience {
        Consumer,
        Business
    }

    /// <summary>
    /// Only available if the payment has been completed – The type of security used during payment processing.
    /// </summary>
    public enum CreditCardSecurity {
        Normal,
        [EnumMember(Value = "3dsecure")]
        Secure3D
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
    public enum CreditCardLabel {
        [EnumMember(Value = "American Express")] AmericanExpress,
        [EnumMember(Value = "Carta si")] CartaSi,
        [EnumMember(Value = "Carte Bleue")] CarteBleue,
        Dankort,
        [EnumMember(Value = "Diners Club")] DinersClub,
        Discover,
        [EnumMember(Value = "JCB")] Jcb,
        [EnumMember(Value = "Laser")] Laser,
        Maestro,
        Mastercard,
        Unionpay,
        Visa
    }
}