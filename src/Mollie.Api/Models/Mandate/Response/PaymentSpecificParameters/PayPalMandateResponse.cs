namespace Mollie.Api.Models.Mandate.Response.PaymentSpecificParameters;

public record PayPalMandateResponse : MandateResponse {
    public required PayPalMandateResponseDetails Details { get; set; }
}

public record PayPalMandateResponseDetails {
    /// <summary>
    /// Only available if the payment has been completed – The consumer's name.
    /// </summary>
    public string? ConsumerName { get; set; }

    /// <summary>
    /// Only available if the payment has been completed – The consumer's IBAN.
    /// </summary>
    public string? ConsumerAccount { get; set; }
}
