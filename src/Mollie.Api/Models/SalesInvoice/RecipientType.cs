namespace Mollie.Api.Models.SalesInvoice;

/// <summary>
/// The type of recipient, either consumer or business. This will determine what further fields are required on the
/// recipient object.
/// </summary>
public static class RecipientType {
    /// <summary>
    /// The recipient is a consumer.
    /// </summary>
    public const string Consumer = "consumer";

    /// <summary>
    /// The recipient is a business.
    /// </summary>
    public const string Business = "business";
}
