using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Request;
using Mollie.WebApplicationExample.Infrastructure;
using Mollie.WebApplicationExample.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mollie.WebApplicationExample.Controllers
{
    public class MandateController : Controller
    {
        private readonly IMollieClient _mollieClient;

        public MandateController()
        {
            this._mollieClient = new MollieClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index(string customerId)
        {
            ViewBag.CustomerId = customerId;

            ListResponse<MandateResponse> mandateList = await this._mollieClient.GetMandateListAsync(customerId);
            return this.View(mandateList.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string customerId, string mandateId)
        {
            MandateResponse mandate = await this._mollieClient.GetMandateAsync(customerId, mandateId);
            return this.View(mandate);
        }

        [HttpGet]
        public async Task<ActionResult> Create(string customerId)
        {
            // You need at least 1 payment to make a new mandate
            var result = await this._mollieClient.CreatePaymentAsync(new PaymentRequest()
            {
                Amount = 1,
                Description = "First payment",
                Locale = Api.Models.Payment.Locale.NL,
                CustomerId = customerId,
                RecurringType = Api.Models.Payment.RecurringType.First,
                RedirectUrl = @"http://www.google.nl"
            });


            return this.Redirect(result.Links.PaymentUrl);
        }

        [HttpGet]
        public ActionResult CreateManual(string customerId)
        {
            MandateRequestModel mandateRequest = new MandateRequestModel { };
            mandateRequest.CustomerId = customerId;
            return this.View(mandateRequest);
        }

        [HttpPost]
        public async Task<ActionResult> CreateManual(string customerId, MandateRequestModel model)
        {
            if(this.ModelState.IsValid)
            {
                MandateRequest mandateRequest = new MandateRequest();
                mandateRequest.ConsumerName = model.ConsumerName;
                mandateRequest.ConsumerAccount = model.ConsumerAccount;
                mandateRequest.ConsumerBic = model.ConsumerBic;
                mandateRequest.SignatureDate = model.SignatureDate;
                mandateRequest.MandateReference = model.MandateReference;

                await this._mollieClient.CreateMandateAsync(model.CustomerId, mandateRequest);
                return this.RedirectToAction("Index", new { customerId = customerId });
            }

            return View(model);
        }
    }
}