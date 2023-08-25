using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Permission;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class PermissionsClient : OauthBaseMollieClient, IPermissionsClient {
        public PermissionsClient(string oauthAccessToken, HttpClient httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public async Task<PermissionResponse> GetPermissionAsync(string permissionId) {
            this.ValidateRequiredUrlParameter(nameof(permissionId), permissionId);
            return await this.GetAsync<PermissionResponse>($"permissions/{permissionId}").ConfigureAwait(false);
        }

        public async Task<PermissionResponse> GetPermissionAsync(UrlObjectLink<PermissionResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PermissionResponse>> GetPermissionListAsync() {
            return await this.GetListAsync<ListResponse<PermissionResponse>>("permissions", null, null).ConfigureAwait(false);
        }
    }
}