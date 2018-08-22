using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Overview;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class CustomerController : Controller {
        private readonly IOverviewClient<CustomerResponse> _customerOverviewClient;

        public CustomerController(IOverviewClient<CustomerResponse> customerOverviewClient) {
            this._customerOverviewClient = customerOverviewClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetList();
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetList(url);
            return this.View(nameof(this.Index), model);
        }
    }
}