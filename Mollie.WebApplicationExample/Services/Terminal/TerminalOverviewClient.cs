using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Terminal;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Terminal;

public class TerminalOverviewClient : OverviewClientBase<TerminalResponse>, ITerminalOverviewClient {
    private readonly ITerminalClient _terminalClient;

    public TerminalOverviewClient(IMapper mapper, ITerminalClient terminalClient) : base(mapper) {
        _terminalClient = terminalClient;
    }

    public async Task<OverviewModel<TerminalResponse>> GetList() {
        return this.Map(await this._terminalClient.GetTerminalListAsync());
    }

    public async Task<OverviewModel<TerminalResponse>> GetListByUrl(string url) {
        return this.Map(await this._terminalClient.GetTerminalListAsync(this.CreateUrlObject(url)));
    }
}