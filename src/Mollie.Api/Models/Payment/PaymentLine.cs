namespace Mollie.Api.Models.Payment;

public record PaymentLine {
    /// <summary>
    /// The type of product purchased. For example, a physical or a digital product.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// A description of the line item. For example LEGO 4440 Forest Police Station.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The number of items.
    /// </summary>
    public required int Quantity { get; init; }

    /// <summary>
    /// The unit for the quantity. For example pcs, kg, or cm.
    /// </summary>
    public required string QuantityUnit { get; init; }

    /// <summary>
    /// The price of a single item including VAT. The unit price can be zero in case of free items.
    /// </summary>
    public required Amount UnitPrice { get; init; }

    /// <summary>
    /// The total amount of the line, including VAT and discounts.
    /// </summary>
    public required Amount TotalAmount { get; init; }

    /// <summary>
    /// Any line-specific discounts, as a positive amount. Not relevant if the line itself is already a discount type.
    /// </summary>
    public Amount? DiscountAmount { get; set; }

    /// <summary>
    /// The VAT rate applied to the line, for example 21.00 for 21%. The vatRate should be passed as a string and not
    /// as a float, to ensure the correct number of decimals are passed.
    /// </summary>
    public string? VatRate { get; set; } // TODO: make it decimal?

    /// <summary>
    /// The amount of value-added tax on the line. The totalAmount field includes VAT, so the vatAmount can be
    /// calculated with the formula totalAmount × (vatRate / (100 + vatRate)).
    /// </summary>
    public Amount? VatAmount { get; set; }

    /// <summary>
    /// The SKU, EAN, ISBN or UPC of the product sold.
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// A link pointing to an image of the product sold.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// A link pointing to the product page in your web shop of the product sold.
    /// </summary>
    public string? ProductUrl { get; set; }
}
