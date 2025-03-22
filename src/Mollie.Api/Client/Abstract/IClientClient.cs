using System.Threading.Tasks;
using Mollie.Api.Models.Client.Response;
using Mollie.Api.Models.List.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IClientClient : IBaseMollieClient
    {
        Task<ClientResponse> GetClientAsync(
            string clientId, bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false);

        Task<ListResponse<ClientResponse>> GetClientListAsync(
            string? from = null, int? limit = null, bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false);
    }
}
