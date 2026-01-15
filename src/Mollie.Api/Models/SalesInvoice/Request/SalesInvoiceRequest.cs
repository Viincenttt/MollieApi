using System.Collections.Generic;

namespace Mollie.Api.Models.SalesInvoice.Request;

public record SalesInvoiceRequest : ITestModeRequest, IProfileRequest {
    /// <summary>
    ///	Whether to create the entity in test mode or live mode. Most API credentials are specifically created for
    /// either live mode or test mode, in which case this parameter can be omitted. For organization-level credentials
    /// such as OAuth access tokens, you can enable test mode by setting testmode to true.
    /// </summary>
    public bool? Testmode { get; set; }

    /// <summary>
    /// The identifier referring to the profile this entity belongs to. Most API credentials are linked to a single
    /// profile. In these cases the profileId can be omitted in the creation request. For organization-level
    /// credentials such as OAuth access tokens however, the profileId parameter is required.
    /// </summary>
    public string? ProfileId { get; set; }

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
    public required string PaymentTerm { get; set; }

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
    public required Recipient Recipient { get; set; }

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
}
