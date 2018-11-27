using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.PaymentMethod;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.PaymentMethod;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class PaymentMethodController : Controller {
        private readonly IPaymentMethodOverviewClient _paymentMethodOverviewClient;

        public PaymentMethodController(IPaymentMethodOverviewClient paymentMethodOverviewClient) {
            this._paymentMethodOverviewClient = paymentMethodOverviewClient;
        }

        public async Task<ViewResult> Index() {
            OverviewModel<PaymentMethodResponse> model = await this._paymentMethodOverviewClient.GetList();
            return this.View(model);
        }
    }
}