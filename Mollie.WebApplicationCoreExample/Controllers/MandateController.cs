using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Mandate;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Mandate;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class MandateController : Controller {
        private readonly IMandateOverviewClient _mandateOverviewClient;
        public MandateController(IMandateOverviewClient mandateOverviewClient) {
            this._mandateOverviewClient = mandateOverviewClient;
        }

        public async Task<ViewResult> Index(string customerId) {
            OverviewModel<MandateResponse> model = await this._mandateOverviewClient.GetList(customerId);
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<MandateResponse> model = await this._mandateOverviewClient.GetListByUrl(url);
            return this.View(nameof(this.Index), model);
        }
    }
}