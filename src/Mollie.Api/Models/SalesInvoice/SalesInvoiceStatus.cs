namespace Mollie.Api.Models.SalesInvoice;

/// <summary>
/// Known sales invoice statuses. All values can be returned by the API. When creating an invoice, only draft, issued
/// and paid can be used. When updating an invoice, draft, issued, paid, cancelled and payment-reversed can be used.
/// To cancel a paid invoice, first set it to payment-reversed and then to cancelled.
/// </summary>
public static class SalesInvoiceStatus {
    public const string Draft = "draft";
    public const string Issuing = "issuing";
    public const string Issued = "issued";
    public const string PendingPayment = "pending-payment";
    public const string Paid = "paid";
    public const string Overdue = "overdue";
    public const string PaymentReversed = "payment-reversed";
    public const string Cancelled = "cancelled";
    public const string Expired = "expired";
    public const string Failed = "failed";
}
