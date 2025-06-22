using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.SalesInvoice.Response;

public record SalesInvoiceResponse {
    /// <summary>
    /// Indicates the response contains a sales invoice object. Will always contain the string sales-invoice for
    /// this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this invoice. Example: invoice_4Y0eZitmBnQ6IDoMqZQKh.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// When issued, an invoice number will be set for the sales invoice.
    /// </summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// The identifier referring to the profile this entity belongs to.
    /// </summary>
    public string? ProfileId { get; set; }

    /// <summary>
    /// A three-character ISO 4217 currency code.
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// The status for the invoice to end up in. See the Mollie.Api.Models.SalesInvoice.SalesInvoiceStatus class for
    /// a full list of known values.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The VAT scheme to create the invoice for. You must be enrolled with One Stop Shop enabled to use it. See the
    /// Mollie.Api.Models.VatScheme class for a full list of known values.
    /// </summary>
    public string? VatScheme { get; set; }

    /// <summary>
    /// The VAT mode to use for VAT calculation. exclusive mode means we will apply the relevant VAT on top of the
    /// price. inclusive means the prices you are providing to us already contain the VAT you want to apply. See the
    /// Mollie.Api.Models.VatMode class for a full list of known values.
    /// </summary>
    public string? VatMode { get; set; }

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
    /// Used when setting an invoice to status of paid, and will store a payment that fully pays the invoice with
    /// the provided details. Required for paid status.
    /// </summary>
    public PaymentDetails? PaymentDetails { get; set; }

    /// <summary>
    /// Used when setting an invoice to status of either issued or paid. Will be used to issue the invoice to the
    /// recipient with the provided subject and body. Required for issued status.
    /// </summary>
    public EmailDetails? EmailDetails { get; set; }

    /// <summary>
    /// The identifier referring to the customer you want to attempt an automated payment for. If provided,
    /// mandateId becomes required as well. Only allowed for invoices with status paid.
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// The identifier referring to the mandate you want to use for the automated payment. If provided, customerId
    /// becomes required as well. Only allowed for invoices with status paid.
    /// </summary>
    public string? MandateId { get; set; }

    /// <summary>
    /// An identifier tied to the recipient data. This should be a unique value based on data your system contains,
    /// so that both you and us know who we're referring to. It is a value you provide to us so that recipient
    /// management is not required to send a first invoice to a recipient.
    /// </summary>
    public required string RecipientIdentifier { get; set; }

    /// <summary>
    /// The recipient details
    /// </summary>
    public Recipient? Recipient { get; set; }

    /// <summary>
    /// Provide the line items for the invoice. Each line contains details such as a description of the item ordered
    /// and its price. All lines must have the same currency as the invoice.
    /// </summary>
    public required IEnumerable<SalesInvoiceLine>? Lines { get; set; }

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

    /// <summary>
    /// The amount that is left to be paid.
    /// </summary>
    public required Amount AmountDue { get; set; }

    /// <summary>
    /// The total amount without VAT.
    /// </summary>
    public required Amount SubtotalAmount { get; set; }

    /// <summary>
    /// The total amount with VAT.
    /// </summary>
    public required Amount TotalAmount { get; set; }

    /// <summary>
    /// The discounted subtotal amount without VAT.
    /// </summary>
    public required Amount DiscountedSubtotalAmount { get; set; }

    /// <summary>
    /// The entity's date and time of creation.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// If issued, the date when the sales invoice was issued.
    /// </summary>
    public DateTime? IssuedAt { get; set; }

    /// <summary>
    /// If issued, the date when the sales invoice payment is due.
    /// </summary>
    public DateTime? DueAt { get; set; }

    /// <summary>
    /// An object with several relevant URLs. Every URL object will contain an href and a type field.
    /// </summary>
    [JsonPropertyName("_links")]
    public required SalesInvoiceResponseLinks Links { get; set; }
}
