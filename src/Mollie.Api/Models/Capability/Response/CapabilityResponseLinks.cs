using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Capability.Response;

public record CapabilityResponseLinks {
    public UrlLink? Documentation { get; set; }
}
