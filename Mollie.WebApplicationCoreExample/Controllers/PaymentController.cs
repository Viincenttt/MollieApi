using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Overview;
using Mollie.WebApplicationCoreExample.Services.Payment;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class PaymentController : Controller {
        private readonly IPaymentOverviewClient _paymentOverviewClient;
        private readonly IPaymentStorageClient _paymentStorageClient;

        public PaymentController(IPaymentOverviewClient paymentOverviewClient, IPaymentStorageClient paymentStorageClient) {
            this._paymentOverviewClient = paymentOverviewClient;
            this._paymentStorageClient = paymentStorageClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            OverviewModel<PaymentResponse> model = await this._paymentOverviewClient.GetList();
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<PaymentResponse> model = await this._paymentOverviewClient.GetListByUrl(url);
            return this.View(nameof(this.Index), model);
        }

        [HttpGet]
        public ViewResult Create() {
            CreatePaymentModel model = new CreatePaymentModel() {
                Currency = Currency.EUR
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentModel model) {
            if (!this.ModelState.IsValid) {
                return this.View();
            }

            await this._paymentStorageClient.Create(model);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}