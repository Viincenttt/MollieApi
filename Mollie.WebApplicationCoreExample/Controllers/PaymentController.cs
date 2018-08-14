using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Url;

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

        public async Task<ViewResult> Next([FromQuery]string url) {
            return await this.GetListByUrl(url);
        }

        public async Task<ViewResult> Previous([FromQuery] string url) {
            return await this.GetListByUrl(url);
        }

        private async Task<ViewResult> GetListByUrl(string url) {
            UrlObjectLink<ListResponse<PaymentListData>> urlObject = new UrlObjectLink<ListResponse<PaymentListData>>() {
                Href = url
            };

            ListResponse<PaymentListData> paymentList = await this._paymentClient.GetPaymentListAsync(urlObject);
            return this.View("Index", paymentList);
        }
    }
}