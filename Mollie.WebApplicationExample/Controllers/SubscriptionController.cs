using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationExample.Infrastructure;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Controllers {
    public class SubscriptionController : Controller {
        private readonly ISubscriptionClient _subscriptionClient;

        public SubscriptionController() {
            this._subscriptionClient = new SubscriptionClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index(string customerId) {
            ListResponse<SubscriptionResponse> subscriptions = await this._subscriptionClient.GetSubscriptionListAsync(customerId);
            SubscriptionListViewModel viewModel = new SubscriptionListViewModel() {
                CustomerId = customerId,
                Subscriptions = subscriptions.Data
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(string customerId, string subscriptionId) {
            await this._subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionId);
            return this.RedirectToAction("Index", new { customerId });
        }

        [HttpGet]
        public ActionResult Create(string customerId) {
            SubscriptionRequestModel subscriptionRequest = new SubscriptionRequestModel();
            subscriptionRequest.CustomerId = customerId;
            return this.View("Create", subscriptionRequest);
        }

        public async Task<ActionResult> Create(SubscriptionRequestModel subscriptionRequestModel) {
            if (this.ModelState.IsValid) {
                SubscriptionRequest subscriptionRequest = new SubscriptionRequest();
                subscriptionRequest.Amount = subscriptionRequestModel.Amount;
                subscriptionRequest.Description = subscriptionRequestModel.Description;
                subscriptionRequest.Interval = "14 days";
                await this._subscriptionClient.CreateSubscriptionAsync(subscriptionRequestModel.CustomerId, subscriptionRequest);

                return this.RedirectToAction("Index", new { customerId = subscriptionRequestModel.CustomerId });
            }

            return this.View(subscriptionRequestModel);
        }
    }
}