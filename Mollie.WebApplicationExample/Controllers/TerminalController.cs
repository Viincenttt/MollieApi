using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Terminal;
using Mollie.WebApplicationExample.Models;
using Mollie.WebApplicationExample.Services.Terminal;

namespace Mollie.WebApplicationExample.Controllers; 

public class TerminalController : Controller {
    private readonly ITerminalOverviewClient _terminalOverviewClient;

    public TerminalController(ITerminalOverviewClient terminalOverviewClient) {
        _terminalOverviewClient = terminalOverviewClient;
    }
    
    [HttpGet]
    public async Task<ViewResult> Index() {
        OverviewModel<TerminalResponse> model = await this._terminalOverviewClient.GetList();
        return this.View(model);
    }
}