using System.Threading.Tasks;
using Mollie.Api.Models.Capability.Response;
using Mollie.Api.Models.List.Response;

namespace Mollie.Api.Client.Abstract;

public interface ICapabilityClient {
    Task<ListResponse<CapabilityResponse>> GetCapabilitiesListAsync();
}
