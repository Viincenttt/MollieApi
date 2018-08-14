using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class PaymentController : Controller {
        private readonly IPaymentClient _paymentClient;

        public PaymentController(IPaymentClient paymentClient) {
            this._paymentClient = paymentClient;
        }

        public async Task<ViewResult> Index() {
            ListResponse<PaymentListData> paymentList = await this._paymentClient.GetPaymentListAsync();
            return this.View(paymentList);
        }
    }
}