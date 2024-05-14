﻿using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract
{    /// <summary>
     /// Calls in this class are documented in https://docs.mollie.com/reference/v2/terminals-api/overview
     /// </summary>
    public interface ITerminalClient : IBaseMollieClient {
        Task<TerminalResponse> GetTerminalAsync(string terminalId);
        Task<TerminalResponse> GetTerminalAsync(UrlObjectLink<TerminalResponse> url);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(string? from = null, int? limit = null, string? profileId = null, bool testmode = false);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(UrlObjectLink<ListResponse<TerminalResponse>> url);
     }
}
