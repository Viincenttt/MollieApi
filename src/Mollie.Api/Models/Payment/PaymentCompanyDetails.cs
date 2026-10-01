namespace Mollie.Api.Models.Payment;

public record PaymentCompanyDetails {
    /// <summary>
    /// The organization's registration number.
    /// </summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// The organization's VAT number.
    /// </summary>
    public string? VatNumber { get; set; }

    /// <summary>
    /// The organization's entity type.
    /// </summary>
    public string? EntityType { get; set; }
}
