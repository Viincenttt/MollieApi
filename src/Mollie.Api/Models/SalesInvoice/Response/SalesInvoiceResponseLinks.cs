using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.SalesInvoice.Response;

public record SalesInvoiceResponseLinks {
    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public required UrlObjectLink<SalesInvoiceResponse> Self { get; set; }

    /// <summary>
    /// The URL your customer should visit to make payment for the invoice. This is where you should redirect the
    /// customer to unless the status is set to paid.
    /// </summary>
    public required UrlLink InvoicePayment { get; set; }

    /// <summary>
    /// The URL the invoice is available at, if generated.
    /// </summary>
    public UrlLink? PdfLink { get; set; }

    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public UrlLink? Documentation { get; set; }
}
