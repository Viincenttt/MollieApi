using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Mandate;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Mandate;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class MandateController : Controller {
        private readonly IMandateOverviewClient _mandateOverviewClient;
        private readonly IMandateStorageClient _mandateStorageClient;

        public MandateController(IMandateOverviewClient mandateOverviewClient, IMandateStorageClient mandateStorageClient) {
            this._mandateOverviewClient = mandateOverviewClient;
            this._mandateStorageClient = mandateStorageClient;
        }

        public async Task<ViewResult> Index(string customerId) {
            this.ViewBag.CustomerId = customerId;
            OverviewModel<MandateResponse> model = await this._mandateOverviewClient.GetList(customerId);
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<MandateResponse> model = await this._mandateOverviewClient.GetListByUrl(url);
            return this.View(nameof(this.Index), model);
        }

        [HttpGet]
        public ViewResult Create(string customerId) {
            this.ViewBag.CustomerId = customerId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string customerId) {
            await this._mandateStorageClient.Create(customerId);
            return this.RedirectToAction("Index", new { customerId });
        }
    }
}