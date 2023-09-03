using System;
using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Permission;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPermissionsClient : IDisposable {
        Task<PermissionResponse> GetPermissionAsync(string permissionId);
        Task<PermissionResponse> GetPermissionAsync(UrlObjectLink<PermissionResponse> url);
        Task<ListResponse<PermissionResponse>> GetPermissionListAsync();
    }
}