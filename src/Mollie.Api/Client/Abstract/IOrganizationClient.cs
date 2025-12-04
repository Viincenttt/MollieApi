using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationClient : IBaseMollieClient {
        Task<OrganizationResponse> GetCurrentOrganizationAsync(CancellationToken cancellationToken = default);
        Task<OrganizationResponse> GetOrganizationAsync(string organizationId, CancellationToken cancellationToken = default);
        Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url, CancellationToken cancellationToken = default);
        Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url, CancellationToken cancellationToken = default);
        Task<PartnerResponse> GetPartnerStatus(CancellationToken cancellationToken = default);
    }
}
