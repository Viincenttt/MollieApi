using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Subscription;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class SubscriptionController : Controller {
        private readonly ISubscriptionOverviewClient _subscriptionOverviewClient;
        private readonly ISubscriptionStorageClient _subscriptionStorageClient;

        public SubscriptionController(ISubscriptionOverviewClient subscriptionOverviewClient, ISubscriptionStorageClient subscriptionStorageClient) {
            this._subscriptionOverviewClient = subscriptionOverviewClient;
            this._subscriptionStorageClient = subscriptionStorageClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index(string customerId) {
            this.ViewBag.CustomerId = customerId;
            OverviewModel<SubscriptionResponse> model = await this._subscriptionOverviewClient.GetList(customerId);
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<SubscriptionResponse> model = await this._subscriptionOverviewClient.GetListByUrl(url);
            return this.View(nameof(this.Index), model);
        }

        [HttpGet]
        public ViewResult Create(string customerId) {
            CreateSubscriptionModel model = new CreateSubscriptionModel() {
                CustomerId = customerId,
                Amount = 10,
                Currency = Currency.EUR
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubscriptionModel model) {
            if (!this.ModelState.IsValid) {
                return this.View();
            }

            await this._subscriptionStorageClient.Create(model);
            return this.RedirectToAction(nameof(this.Index), new { customerId = model.CustomerId });
        }
    }
}