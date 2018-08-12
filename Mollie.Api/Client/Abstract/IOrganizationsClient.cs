using System.Threading.Tasks;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationsClient {
        Task<OrganizationResponse> GetOrganizationAsync(string organizationId);
        Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url);
    }
}