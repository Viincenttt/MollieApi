namespace Mollie.Api.Models.SalesInvoice;

/// <summary>
/// The way through which the invoice is to be set to paid.
/// </summary>
public static class PaymentDetailsSource {
    /// <summary>
    /// The invoice is to be paid manually.
    /// </summary>
    public const string Manual = "manual";

    /// <summary>
    /// The invoice is to be paid through a payment link.
    /// </summary>
    public const string PaymentLink = "payment-link";

    /// <summary>
    /// The invoice is to be paid through a payment.
    /// </summary>
    public const string Payment = "payment";
}
