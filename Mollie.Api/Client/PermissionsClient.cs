using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Permission;

namespace Mollie.Api.Client
{
	public class PermissionsClient : OauthBaseMollieClient, IPermissionsClient
	{
		public PermissionsClient(string oauthAccessToken) : base(oauthAccessToken) { }

		public async Task<PermissionResponse> GetPermissionAsync(string permissionId)
		{
			return await this.GetAsync<PermissionResponse>($"permissions/{permissionId}").ConfigureAwait(false);
		}

		public async Task<ListResponse<PermissionResponse>> GetPermissionListAsync(int? offset = null, int? count = null)
		{
			return await this.GetListAsync<ListResponse<PermissionResponse>>("permissions", offset, count).ConfigureAwait(false);
		}
	}
}