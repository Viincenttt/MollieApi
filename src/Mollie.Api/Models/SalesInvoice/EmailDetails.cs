namespace Mollie.Api.Models.SalesInvoice;

public record EmailDetails {
    /// <summary>
    /// The subject of the email to be sent.
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// The body of the email to be sent. To add newline characters, you can use \n.
    /// </summary>
    public required string Body { get; set; }
}
