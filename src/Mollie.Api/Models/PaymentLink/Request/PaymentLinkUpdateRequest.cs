using System.Collections.Generic;

namespace Mollie.Api.Models.PaymentLink.Request;

public record PaymentLinkUpdateRequest {
    /// <summary>
    /// A short description of the payment link. The description is visible in the Dashboard and will be shown on the
    /// customer's bank or card statement when possible.
    ///
    /// Updating the description does not affect any previously existing payments created for this payment link.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Whether the payment link is archived. Customers will not be able to complete payments on archived payment links.
    /// </summary>
    public required bool Archived { get; init; }

    /// <summary>
    /// An array of payment methods that are allowed to be used for this payment link. When this parameter is not
    /// provided or is an empty array, all enabled payment methods will be available.
    /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
    /// </summary>
    public IEnumerable<string>? AllowedMethods { get; set; }
}
