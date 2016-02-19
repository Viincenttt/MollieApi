using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationExample.Infrastructure;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Controllers {
    public class PaymentController : Controller {
        private const int NumberOfPaymentsToList = 50;
        private readonly MollieClient _mollieClient;

        public PaymentController() {
            this._mollieClient = new MollieClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index() {
            ListResponse<PaymentResponse> paymentList = await this._mollieClient.GetPaymentListAsync(0, NumberOfPaymentsToList);
            return View(paymentList.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id) {
            PaymentResponse payment = await this._mollieClient.GetPaymentAsync(id);
            return View(payment);
        }

        [HttpPost]
        public async Task<ActionResult> Pay(string id) {
            PaymentResponse payment = await this._mollieClient.GetPaymentAsync(id);

            return this.Redirect(payment.Links.PaymentUrl);
        }

        [HttpGet]
        public ActionResult Create() {
            PaymentRequestModel payment = new PaymentRequestModel();
            return this.View(payment);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PaymentRequestModel paymentRequestModel) {
            if (this.ModelState.IsValid) {
                PaymentRequest paymentRequest = new PaymentRequest();
                paymentRequest.Amount = paymentRequestModel.Amount;
                paymentRequest.Description = paymentRequestModel.Description;
                paymentRequest.RedirectUrl = paymentRequestModel.RedirectUrl;
                await this._mollieClient.CreatePaymentAsync(paymentRequest);

                return this.RedirectToAction("Index");
            }

            return this.View(paymentRequestModel);
        }
    }
}