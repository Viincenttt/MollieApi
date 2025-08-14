namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;

public record PointOfSalePaymentResponse : PaymentResponse {
    /// <summary>
    /// An object with payment details.
    /// </summary>
    public required PointOfSalePaymentResponseDetails? Details { get; set; }
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
    /// The first 6 digits & last 4 digits of the customer's masked card number.
    /// </summary>
    public string? MaskedNumber { get; set; }

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
    /// The card funding type, if known. Possible values: credit, debit
    /// </summary>
    public string? CardFunding { get; set; }

    /// <summary>
    /// Only available if the payment has been completed - The ISO 3166-1 alpha-2 country code of the country
    /// the card was issued in. For example: BE.
    /// </summary>
    public string? CardCountryCode { get; set; }

    /// <summary>
    /// The applicable card fee region. For example, intra-eea applies to consumer cards from the European Economic
    /// Area (EEA). See the Mollie.Api.Models.Payment.Response.CreditCardFeeRegion class for a full list of known values
    /// </summary>
    public string? FeeRegion { get; set; }

    /// <summary>
    /// The Point of sale receipt object.
    /// </summary>
    public PointOfSaleReceipt? Receipt { get; set; }

    /// <summary>
    /// Beta feature: The entrymode of the payment. See the Mollie.Api.Models.Payment.EntryMode class for a full
    /// list of known values
    /// </summary>
    public string? EntryMode { get; set; }
}

public record PointOfSaleReceipt {
    /// <summary>
    /// A unique code provided by the cardholder’s bank to confirm that the transaction was successfully approved.
    /// </summary>
    public string? AuthorizationCode { get; set; }

    /// <summary>
    /// The unique number that identifies a specific payment application on a chip card.
    /// </summary>
    public string? ApplicationIdentifier { get; set; }

    /// <summary>
    /// The method by which the card was read by the terminal.
    /// See the Mollie.Api.Models.Payment.Response.PaymentSpecificParameters.CardReadMethod class for a full list of known values
    /// </summary>
    public string? CardReadMethod { get; set; }

    /// <summary>
    /// The method used to verify the cardholder's identity.
    /// See the Mollie.Api.Models.Payment.Response.PaymentSpecificParameters.CardVerificationMethod class for a full list of known values
    /// </summary>
    public string? CardVerificationMethod { get; set; }
}

/// <summary>
/// The method by which the card was read by the terminal.
/// </summary>
public static class CardReadMethod {
    public const string IntegratedCircuitCard = "integrated-circuit-card";
    public const string MagneticStripeReader = "magnetic-stripe-reader";
    public const string NearFieldCommunication = "near-field-communication";
    public const string Contactless  = "contactless ";
    public const string Manual  = "manual ";
}

/// <summary>
/// the method used to verify the cardholder's identity.
/// </summary>
public static class CardVerificationMethod {
    public const string NoCvmRequired = "no-cvm-required";
    public const string OnlinePin = "online-pin";
    public const string Signature = "signature";
    public const string SignatureAndOnlinePin = "signature-and-online-pin";
    public const string OnlinePinAndSignature = "online-pin-and-signature";
    public const string None = "none";
    public const string Failed = "failed";
}
