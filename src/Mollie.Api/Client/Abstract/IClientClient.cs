using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Client.Response;
using Mollie.Api.Models.List.Response;
using System.Threading;

namespace Mollie.Api.Client.Abstract {
    public interface IClientClient : IBaseMollieClient
    {
        Task<MollieResult<ClientResponse>> GetClientAsync(
            string clientId, bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false, CancellationToken cancellationToken = default);

        Task<MollieResult<ListResponse<ClientResponse>>> GetClientListAsync(
            string? from = null, int? limit = null, bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false, CancellationToken cancellationToken = default);
    }
}
