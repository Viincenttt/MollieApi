using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationClient : IBaseMollieClient {
        Task<MollieResult<OrganizationResponse>> GetCurrentOrganizationAsync(CancellationToken cancellationToken = default);
        Task<MollieResult<OrganizationResponse>> GetOrganizationAsync(string organizationId, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<OrganizationResponse>>> GetOrganizationListAsync(string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<OrganizationResponse>>> GetOrganizationListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url, CancellationToken cancellationToken = default);
        Task<MollieResult<OrganizationResponse>> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<PartnerResponse>> GetPartnerStatusAsync(CancellationToken cancellationToken = default);
    }
}
