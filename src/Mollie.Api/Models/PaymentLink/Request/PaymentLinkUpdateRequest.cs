using System.Collections.Generic;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.PaymentLink.Request;

public record PaymentLinkUpdateRequest : ITestModeRequest {
    /// <summary>
    /// A short description of the payment link. The description is visible in the Dashboard and will be shown on the
    /// customer's bank or card statement when possible.
    ///
    /// Updating the description does not affect any previously existing payments created for this payment link.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The minimum amount of the payment link. This property is only allowed when there is no amount provided.
    /// The customer will be prompted to enter a value greater than or equal to the minimum amount.
    /// </summary>
    public Amount? MinimumAmount { get; set; }

    /// <summary>
    /// Whether the payment link is archived. Customers will not be able to complete payments on archived payment links.
    /// </summary>
    public required bool Archived { get; set; }

    /// <summary>
    /// An array of payment methods that are allowed to be used for this payment link. When this parameter is not
    /// provided or is an empty array, all enabled payment methods will be available.
    /// See the Mollie.Api.Models.Payment.PaymentMethod class for a full list of known values.
    /// </summary>
    public IEnumerable<string>? AllowedMethods { get; set; }

    /// <summary>
    /// Optionally provide the order lines for the payment. Each line contains details such as a description of the item ordered and its price.
    /// All lines must have the same currency as the payment.
    /// </summary>
    public List<PaymentLine>? Lines { get; set; }

    /// <summary>
    /// The customer's billing address details. We advise to provide these details to improve fraud protection and conversion.
    /// </summary>
    public PaymentAddressDetails? BillingAddress { get; set; }

    /// <summary>
    /// The customer's shipping address details. We advise to provide these details to improve fraud protection and conversion.
    /// </summary>
    public PaymentAddressDetails? ShippingAddress { get; set; }

    /// <summary>
    ///	Oauth only - Optional – Most API credentials are specifically created for either live mode or test mode.
    /// For organization-level credentials such as OAuth access tokens, you can enable test mode by setting testmode
    /// to true.
    ///
    /// Test entities cannot be retrieved when the endpoint is set to live mode, and vice versa.
    /// </summary>
    public bool? Testmode { get; set; }
}
