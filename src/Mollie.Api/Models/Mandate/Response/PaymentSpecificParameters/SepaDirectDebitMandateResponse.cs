namespace Mollie.Api.Models.Mandate.Response.PaymentSpecificParameters;

public record SepaDirectDebitMandateResponse : MandateResponse {
    public required SepaDirectDebitMandateResponseDetails Details { get; set; }
}

public record SepaDirectDebitMandateResponseDetails {
    /// <summary>
    /// Only available if the payment has been completed – The consumer's name.
    /// </summary>
    public string? ConsumerName { get; set; }

    /// <summary>
    /// Only available if the payment has been completed – The consumer's IBAN.
    /// </summary>
    public string? ConsumerAccount { get; set; }

    /// <summary>
    /// Only available if the payment has been completed – The consumer's bank's BIC.
    /// </summary>
    public string? ConsumerBic { get; set; }
}
