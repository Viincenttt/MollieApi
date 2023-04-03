﻿using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Terminals;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract
{    /// <summary>
     /// Calls in this class are documented in https://docs.mollie.com/reference/v2/terminals-api/overview
     /// </summary>
    public interface ITerminalClient {
        Task<TerminalResponse> GetTerminalAsync(string terminalId);
        Task<ListResponse<TerminalResponse>> GetAllTerminalListAsync(string from = null, int? limit = null, string profileId = null);
    }
}
