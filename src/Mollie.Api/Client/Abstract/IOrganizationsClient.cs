using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationsClient : IBaseMollieClient {
        Task<OrganizationResponse> GetCurrentOrganizationAsync();
        Task<OrganizationResponse> GetOrganizationAsync(string organizationId);
        Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(string? from = null, int? limit = null);
        Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url);
        Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url);
    }
}
