using System.Collections.Generic;

namespace Mollie.Api.Models.Capability.Response;

public record CapabilityResponse {
    /// <summary>
    /// Always the word capability for this resource type.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// A unique name for this capability like payments / settlements.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The status of the capability. A list of possible values can be found in the
    /// Mollie.Api.Models.Capability.CapabilityStatus class.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The reason the capability is in this status. A list of possible values can be found in the
    /// Mollie.Api.Models.Capability.CapabilityStatusReason class.
    /// </summary>
    public required string StatusReason { get; set; }

    /// <summary>
    /// The requirements that need to be fulfilled before the capability can be enabled.
    /// </summary>
    public required IEnumerable<CapabilityRequirement> Requirements { get; set; }

    /// <summary>
    /// Related links
    /// </summary>
    public required CapabilityResponseLinks Links { get; set; }
}
