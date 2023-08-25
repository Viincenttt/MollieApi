using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Terminal;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract
{    /// <summary>
     /// Calls in this class are documented in https://docs.mollie.com/reference/v2/terminals-api/overview
     /// </summary>
    public interface ITerminalClient {
        Task<TerminalResponse> GetTerminalAsync(string terminalId);
        Task<TerminalResponse> GetTerminalAsync(UrlObjectLink<TerminalResponse> url);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(string from = null, int? limit = null, string profileId = null, bool testmode = false);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(UrlObjectLink<ListResponse<TerminalResponse>> url);
     }
}
