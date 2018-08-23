using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Subscription;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class SubscriptionController : Controller {
        private readonly ISubscriptionOverviewClient _subscriptionOverviewClient;
        public SubscriptionController(ISubscriptionOverviewClient subscriptionOverviewClient) {
            this._subscriptionOverviewClient = subscriptionOverviewClient;
        }

        public async Task<ViewResult> Index(string customerId) {
            OverviewModel<SubscriptionResponse> model = await this._subscriptionOverviewClient.GetList(customerId);
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<SubscriptionResponse> model = await this._subscriptionOverviewClient.GetListByUrl(url);
            return this.View(nameof(this.Index), model);
        }
    }
}