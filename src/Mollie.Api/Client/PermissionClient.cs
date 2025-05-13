using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Permission.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class PermissionClient : OauthBaseMollieClient, IPermissionClient {
        public PermissionClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public PermissionClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<PermissionResponse> GetPermissionAsync(
            string permissionId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(permissionId), permissionId);
            return await GetAsync<PermissionResponse>(
                $"permissions/{permissionId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PermissionResponse> GetPermissionAsync(
            UrlObjectLink<PermissionResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PermissionResponse>> GetPermissionListAsync(
            CancellationToken cancellationToken = default) {
            return await GetListAsync<ListResponse<PermissionResponse>>(
                "permissions", null, null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
