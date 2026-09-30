namespace Mollie.Api.Models.Webhook;

public static class WebhookEventTypes {
    /// <summary>
    /// A payment link has been paid.
    /// </summary>
    public const string PaymentLinkPaid = "payment-link.paid";

    /// <summary>
    /// A sales invoice has been created.
    /// </summary>
    public const string SalesInvoiceCreated = "sales-invoice.created";

    /// <summary>
    /// A sales invoice has been issued.
    /// </summary>
    public const string SalesInvoiceIssued = "sales-invoice.issued";

    /// <summary>
    /// A sales invoice has been canceled.
    /// </summary>
    public const string SalesInvoiceCanceled = "sales-invoice.canceled";

    /// <summary>
    /// A sales invoice has been paid.
    /// </summary>
    public const string SalesInvoicePaid = "sales-invoice.paid";

    /// <summary>
    /// An unreferenced refund has succeeded.
    /// </summary>
    public const string UnreferencedRefundSucceeded = "unreferenced-refund.succeeded";

    /// <summary>
    /// An unreferenced refund has failed.
    /// </summary>
    public const string UnreferencedRefundFailed = "unreferenced-refund.failed";

    /// <summary>
    /// An unreferenced refund has been canceled.
    /// </summary>
    public const string UnreferencedRefundCanceled = "unreferenced-refund.canceled";

    /// <summary>
    /// An unreferenced refund has expired.
    /// </summary>
    public const string UnreferencedRefundExpired = "unreferenced-refund.expired";

    /// <summary>
    /// All event types
    /// </summary>
    public const string All = "*";
}
