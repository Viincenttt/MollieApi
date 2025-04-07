﻿using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Permission.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPermissionClient : IBaseMollieClient {
        Task<PermissionResponse> GetPermissionAsync(string permissionId,
            CancellationToken cancellationToken = default);
        Task<PermissionResponse> GetPermissionAsync(UrlObjectLink<PermissionResponse> url,
            CancellationToken cancellationToken = default);
        Task<ListResponse<PermissionResponse>> GetPermissionListAsync(
            CancellationToken cancellationToken = default);
    }
}
