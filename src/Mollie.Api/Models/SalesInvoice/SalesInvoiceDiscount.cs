namespace Mollie.Api.Models.SalesInvoice;

public record SalesInvoiceDiscount {
    /// <summary>
    /// The type of discount. See the Mollie.Api.Models.SalesInvoice.SalesInvoiceDiscountType class for a full list of
    /// known values.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// A string containing an exact monetary amount in the given currency, or the percentage.
    /// </summary>
    public required string Value { get; set; }
}
