using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Permission;

namespace Mollie.Api.Client.Abstract {
    public interface IPermissionsClient {
        Task<PermissionResponse> GetPermissionAsync(string permissionId);

        Task<ListResponse<PermissionResponse>> GetPermissionListAsync(string from = null, int? limit = null);
    }
}