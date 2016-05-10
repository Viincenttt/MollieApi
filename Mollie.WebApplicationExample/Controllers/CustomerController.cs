using Mollie.Api.Client;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationExample.Infrastructure;
using Mollie.WebApplicationExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mollie.WebApplicationExample.Controllers
{
    public class CustomerController : Controller
    {
        private const int NumberOfPaymentsToList = 50;
        private readonly MollieClient _mollieClient;

        public CustomerController()
        {
            this._mollieClient = new MollieClient(AppSettings.MollieApiKey);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ListResponse<CustomerResponse> paymentList = await this._mollieClient.GetCustomerListAsync(0, NumberOfPaymentsToList);
            return View(paymentList.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            CustomerResponse payment = await this._mollieClient.GetCustomer(id);
            return View(payment);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CustomerRequestModel payment = new CustomerRequestModel();
            return this.View(payment);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerRequestModel customerRequestModel)
        {
            if (this.ModelState.IsValid)
            {
                var response = await this._mollieClient.CreateCustomer(customerRequestModel.Name, customerRequestModel.Email, customerRequestModel.Locale);

                return this.RedirectToAction("Index");
            }

            return this.View(customerRequestModel);
        }
    }
}