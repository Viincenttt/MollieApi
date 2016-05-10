using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Request;
using Mollie.WebApplicationExample.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mollie.WebApplicationExample.Controllers
{
    public class MandateController : Controller
    {
        private const int NumberOfPaymentsToList = 50;
        private readonly MollieClient _mollieClient;

        public MandateController()
        {
            this._mollieClient = new MollieClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            ViewBag.CustomerId = id;

            ListResponse<MandateResponse> mandateList = await this._mollieClient.GetCustomerMandateListAsync(id, 0, NumberOfPaymentsToList);
            return View(mandateList.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string customerId, string mandateId)
        {
            MandateResponse mandate = await this._mollieClient.GetMandateAsync(customerId, mandateId);
            return View(mandate);
        }

        [HttpGet]
        public async Task<ActionResult> Create(string customerId)
        {
            // You need at least 1 payment to make a new mandate
            var result = await this._mollieClient.CreatePaymentAsync(new RecurringSubscriptionRequest
            {
                Amount = 0.01M,
                CustomerId = customerId,
                Description = "First payment",
                Locale = Api.Models.Payment.Locale.NL,
                RecurringType = Api.Models.Payment.RecurringType.First,
                RedirectUrl = @"http://www.google.nl"
            });


            return Redirect(result.Links.PaymentUrl);
        }
    }
}