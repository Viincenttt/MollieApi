using System;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Capability.Response;

public record CapabilityRequirement {
    /// <summary>
    /// The name of this requirement, referring to the task to be fulfilled by the organization to enable or re-enable
    /// the capability. The name is unique among other requirements of the same capability.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The status of the requirement depends on its due date. If no due date is given, the status will be requested.
    /// A list of possible values can be found in the Mollie.Api.Models.Capability.CapabilityRequirementStatus class.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Due date until the requirement must be fulfilled, if any
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Related links
    /// </summary>
    [JsonProperty("_links")]
    public required CapabilityRequirementLinks Links { get; set; }
}
