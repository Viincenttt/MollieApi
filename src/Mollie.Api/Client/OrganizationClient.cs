using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class OrganizationClient : OauthBaseMollieClient, IOrganizationClient {
        public OrganizationClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public async Task<OrganizationResponse> GetCurrentOrganizationAsync() {
            return await GetAsync<OrganizationResponse>($"organizations/me").ConfigureAwait(false);
        }

        public async Task<OrganizationResponse> GetOrganizationAsync(string organizationId) {
            ValidateRequiredUrlParameter(nameof(organizationId), organizationId);
            return await GetAsync<OrganizationResponse>($"organizations/{organizationId}").ConfigureAwait(false);
        }

        public async Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(string? from = null, int? limit = null) {
            return await GetListAsync<ListResponse<OrganizationResponse>>("organizations", from, limit).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }
    }
}
