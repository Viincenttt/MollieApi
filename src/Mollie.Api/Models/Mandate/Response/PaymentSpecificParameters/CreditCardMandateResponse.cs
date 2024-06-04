namespace Mollie.Api.Models.Mandate.Response.PaymentSpecificParameters;

public record CreditCardMandateResponse : MandateResponse {
    public required CreditCardMandateResponseDetails Details { get; set; }
}

public record CreditCardMandateResponseDetails {
    /// <summary>
    /// The credit card holder's name.
    /// </summary>
    public string? CardHolder { get; set; }

    /// <summary>
    /// The last four digits of the credit card number.
    /// </summary>
    public string? CardNumber { get; set; }

    /// <summary>
    /// The credit card's label. Note that not all labels can be acquired through Mollie.
    /// </summary>
    public string? CardLabel { get; set; }

    /// <summary>
    /// Unique alphanumeric representation of credit card, usable for identifying returning customers.
    /// </summary>
    public string? CardFingerprint { get; set; }

    /// <summary>
    /// Expiry date of the credit card card in YYYY-MM-DD format.
    /// </summary>
    public string? CardExpiryDate { get; set; }
}
