namespace Mollie.Api.Models.SalesInvoice;

public record SalesInvoiceLine {
    /// <summary>
    /// A description of the line item. For example LEGO 4440 Forest Police Station.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The number of items.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// The vat rate to be applied to this line item.
    /// </summary>
    public required string VatRate { get; set; }

    /// <summary>
    /// The price of a single item excluding VAT.
    /// </summary>
    public required Amount UnitPrice { get; set; }

    /// <summary>
    /// The discount to be applied to the line item.
    /// </summary>
    public Amount? Discount { get; set; }
}
