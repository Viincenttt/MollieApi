using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Capability.Response;

public record CapabilityResponseLinks {
    public required UrlLink Documentation { get; set; }
}
