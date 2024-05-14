using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Permission.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPermissionsClient : IBaseMollieClient {
        Task<PermissionResponse> GetPermissionAsync(string permissionId);
        Task<PermissionResponse> GetPermissionAsync(UrlObjectLink<PermissionResponse> url);
        Task<ListResponse<PermissionResponse>> GetPermissionListAsync();
    }
}
