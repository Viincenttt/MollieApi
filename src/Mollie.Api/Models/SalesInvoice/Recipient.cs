namespace Mollie.Api.Models.SalesInvoice;

public record Recipient {
    /// <summary>
    /// The type of recipient, either consumer or business. This will determine what further fields are required on
    /// the recipient object.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// The title of the consumer type recipient, for example Mr. or Mrs..
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The given name (first name) of the consumer type recipient should be at least two characters and cannot contain
    /// only numbers.
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// The given name (last name) of the consumer type recipient should be at least two characters and cannot contain
    /// only numbers.
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// The trading name of the business type recipient.
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// The Chamber of Commerce number of the organization for a business type recipient. Either this or vatNumber has
    /// to be provided.
    /// </summary>
    public string? OrganizationNumber { get; set; }

    /// <summary>
    /// The VAT number of the organization for a business type recipient. Either this or organizationNumber has to be
    /// provided.
    /// </summary>
    public string? VatNumber { get; set; }

    /// <summary>
    /// The email address of the recipient.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The phone number of the recipient.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// A street and street number.
    /// </summary>
    public required string StreetAndNumber { get; set; }

    /// <summary>
    /// Any additional addressing details, for example an apartment number.
    /// </summary>
    public string? StreetAdditional { get; set; }

    /// <summary>
    /// A postal code.
    /// </summary>
    public required string PostalCode { get; set; }

    /// <summary>
    /// The recipient's city.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// The recipient's region.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// A country code in ISO 3166-1 alpha-2 format.
    /// </summary>
    public required string Country { get; set; }

    /// <summary>
    /// The locale for the recipient, to be used for translations in PDF generation and payment pages.
    /// </summary>
    public required string Locale { get; set; }
}
