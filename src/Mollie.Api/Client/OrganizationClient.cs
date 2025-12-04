using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class OrganizationClient : OauthBaseMollieClient, IOrganizationClient {
        public OrganizationClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public OrganizationClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<OrganizationResponse> GetCurrentOrganizationAsync(CancellationToken cancellationToken = default) {
            return await GetAsync<OrganizationResponse>($"organizations/me", cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<OrganizationResponse> GetOrganizationAsync(string organizationId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(organizationId), organizationId);
            return await GetAsync<OrganizationResponse>($"organizations/{organizationId}", cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            return await GetListAsync<ListResponse<OrganizationResponse>>("organizations", from, limit, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrganizationResponse>> GetOrganizationListAsync(UrlObjectLink<ListResponse<OrganizationResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PartnerResponse> GetPartnerStatusAsync(CancellationToken cancellationToken = default) {
            return await GetAsync<PartnerResponse>($"organizations/me/partner", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
