using System.Collections.Generic;

namespace Mollie.Api.Models.SalesInvoice.Request;

public record SalesInvoiceUpdateRequest {
    /// <summary>
    /// The status for the invoice to end up in. Dependent parameters: paymentDetails for paid, emailDetails for issued
    /// and paid. See the Mollie.Api.Models.SalesInvoice.SalesInvoiceStatus class for a full list of known values.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// A free-form memo you can set on the invoice, and will be shown on the invoice PDF.
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// The payment term to be set on the invoice. See the Mollie.Api.Models.SalesInvoice.PaymentTerm class for a full
    /// list of known values.
    /// </summary>
    public string? PaymentTerm { get; set; }

    /// <summary>
    /// Used when setting an invoice to status of paid, and will store a payment that fully pays the invoice with the
    /// provided details. Required for paid status.
    /// </summary>
    public PaymentDetails? PaymentDetails { get; set; }

    /// <summary>
    /// Used when setting an invoice to status of either issued or paid. Will be used to issue the invoice to the
    /// recipient with the provided subject and body. Required for issued status.
    /// </summary>
    public EmailDetails? EmailDetails { get; set; }

    /// <summary>
    /// An identifier tied to the recipient data. This should be a unique value based on data your system contains,
    /// so that both you and us know who we're referring to. It is a value you provide to us so that recipient
    /// management is not required to send a first invoice to a recipient.
    /// </summary>
    public string? RecipientIdentifier { get; set; }

    /// <summary>
    /// The recipient object should contain all the information relevant to create an invoice for an intended recipient.
    /// This data will be stored, updated, and re-used as appropriate, based on the recipientIdentifier.
    /// </summary>
    public Recipient? Recipient { get; set; }

    /// <summary>
    /// Provide the line items for the invoice. Each line contains details such as a description of the item ordered
    /// and its price. All lines must have the same currency as the invoice.
    /// </summary>
    public IEnumerable<SalesInvoiceLine>? Lines { get; set; }

    /// <summary>
    /// The webhook URL where we will send invoice status updates to. The webhookUrl is optional, but without a webhook
    /// you will miss out on important status changes to your invoice. The webhookUrl must be reachable from Mollie's
    /// point of view, so you cannot use localhost. If you want to use webhook during development on localhost, you
    /// must use a tool like ngrok to have the webhooks delivered to your local machine.
    /// </summary>
    public string? WebhookUrl { get; set; }

    /// <summary>
    /// The discount to be applied to the entire invoice, possibly on top of the line item discounts.
    /// </summary>
    public Amount? Discount { get; set; }
}
