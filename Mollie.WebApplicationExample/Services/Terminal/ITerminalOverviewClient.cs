using Mollie.Api.Models.Terminal;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Terminal; 

public interface ITerminalOverviewClient {
    Task<OverviewModel<TerminalResponse>> GetList();
    Task<OverviewModel<TerminalResponse>> GetListByUrl(string url);
}