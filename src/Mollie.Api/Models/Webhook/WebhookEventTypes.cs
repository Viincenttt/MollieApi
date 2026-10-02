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
    /// A payment has been authorized.
    /// </summary>
    public const string PaymentAuthorized = "payment.authorized";

    /// <summary>
    /// A payment has been canceled.
    /// </summary>
    public const string PaymentCanceled = "payment.canceled";

    /// <summary>
    /// A payment has expired.
    /// </summary>
    public const string PaymentExpired = "payment.expired";

    /// <summary>
    /// A payment has failed.
    /// </summary>
    public const string PaymentFailed = "payment.failed";

    /// <summary>
    /// A payment has been paid.
    /// </summary>
    public const string PaymentPaid = "payment.paid";

    /// <summary>
    /// A payment is pending.
    /// </summary>
    public const string PaymentPending = "payment.pending";

    /// <summary>
    /// A payout has been canceled.
    /// </summary>
    public const string PayoutCanceled = "payout.canceled";

    /// <summary>
    /// A payout has been completed.
    /// </summary>
    public const string PayoutCompleted = "payout.completed";

    /// <summary>
    /// A payout has failed.
    /// </summary>
    public const string PayoutFailed = "payout.failed";

    /// <summary>
    /// A payout has been initiated.
    /// </summary>
    public const string PayoutInitiated = "payout.initiated";

    /// <summary>
    /// A payout is being processed at the bank.
    /// </summary>
    public const string PayoutProcessingAtBank = "payout.processing-at-bank";

    /// <summary>
    /// A balance transaction has been created.
    /// </summary>
    public const string BalanceTransactionCreated = "balance-transaction.created";

    /// <summary>
    /// All event types
    /// </summary>
    public const string All = "*";
}
