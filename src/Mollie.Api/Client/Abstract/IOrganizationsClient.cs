﻿using System;
using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Organization;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrganizationsClient : IDisposable {
        Task<OrganizationResponse> GetCurrentOrganizationAsync();
        Task<OrganizationResponse> GetOrganizationAsync(string organizationId);
        Task<ListResponse<OrganizationResponse>> GetOrganizationsListAsync(string from = null, int? limit = null);
        Task<OrganizationResponse> GetOrganizationAsync(UrlObjectLink<OrganizationResponse> url);
    }
}