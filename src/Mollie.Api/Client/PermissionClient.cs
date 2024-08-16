using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Permission.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class PermissionClient : OauthBaseMollieClient, IPermissionClient {
        public PermissionClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public PermissionClient(IBearerTokenRetriever bearerTokenRetriever, HttpClient? httpClient = null) : base(bearerTokenRetriever, httpClient) {
        }

        public async Task<PermissionResponse> GetPermissionAsync(string permissionId) {
            ValidateRequiredUrlParameter(nameof(permissionId), permissionId);
            return await GetAsync<PermissionResponse>($"permissions/{permissionId}").ConfigureAwait(false);
        }

        public async Task<PermissionResponse> GetPermissionAsync(UrlObjectLink<PermissionResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PermissionResponse>> GetPermissionListAsync() {
            return await GetListAsync<ListResponse<PermissionResponse>>("permissions", null, null).ConfigureAwait(false);
        }
    }
}
