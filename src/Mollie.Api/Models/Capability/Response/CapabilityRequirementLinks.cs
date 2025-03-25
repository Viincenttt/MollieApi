using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Capability.Response;

public record CapabilityRequirementLinks {
    /// <summary>
    /// If known, a deep link to the Mollie dashboard of the client, where the requirement can be fulfilled.
    /// For example, where necessary documents are to be uploaded.
    /// </summary>
    public required UrlLink Dashboard { get; set; }
}
