using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.Session;

/// <summary>
/// Kept for backwards compatibility. The recurring details are now available on every payment line as
/// <see cref="PaymentLineRecurringDetails"/>.
/// </summary>
public record SessionLineRecurringDetails : PaymentLineRecurringDetails {
}
