using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationsClient : IBaseMollieClient {
        Task<OrganizationResponse> GetCurrentOrganizationAsync();
        Task<OrganizationResponse> GetOrganizationAsync(string organizationId);
        Task<ListResponse<OrganizationResponse>> GetOrganizationsListAsync(string from = null, int? limit = null);
        Task<ListResponse<OrganizationResponse>> GetOrganizationsListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url);
        Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url);
    }
}