using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.SalesInvoice;

public record SalesInvoiceDiscount {
    /// <summary>
    /// The type of discount. See the Mollie.Api.Models.SalesInvoice.SalesInvoiceDiscountType class for a full list of
    /// known values.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// The exact monetary amount in the currency of the invoice, or the percentage. The value is serialized as a
    /// string to ensure the correct number of decimals are passed, preserving the exact value set by the user.
    /// </summary>
    [JsonConverter(typeof(DecimalToStringConverter))]
    public required decimal Value { get; set; }
}
