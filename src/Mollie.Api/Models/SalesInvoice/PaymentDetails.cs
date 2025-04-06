namespace Mollie.Api.Models.SalesInvoice;

public record PaymentDetails {
    /// <summary>
    /// The way through which the invoice is to be set to paid. See the
    /// Mollie.Api.Models.SalesInvoice.PaymentDetailSource class for a full list of known values.
    /// </summary>
    public required string Source { get; set; }

    /// <summary>
    /// A reference to the payment the sales invoice is paid by. Required for source values payment-link and payment.
    /// </summary>
    public string? SourceReference { get; set; }
}
