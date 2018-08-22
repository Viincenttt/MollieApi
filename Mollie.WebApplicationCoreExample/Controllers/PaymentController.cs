using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Overview;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class PaymentController : Controller {
        private readonly IPaymentClient _paymentClient;
        private readonly IOverviewClient<PaymentResponse> _paymentOverviewClient;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentClient paymentClient, IMapper mapper, IOverviewClient<PaymentResponse> paymentOverviewClient) {
            this._paymentClient = paymentClient;
            this._mapper = mapper;
            this._paymentOverviewClient = paymentOverviewClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            OverviewModel<PaymentResponse> model = await this._paymentOverviewClient.GetList();
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<PaymentResponse> model = await this._paymentOverviewClient.GetList(url);
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

            PaymentRequest paymentRequest = this._mapper.Map<PaymentRequest>(model);
            paymentRequest.RedirectUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            
            await this._paymentClient.CreatePaymentAsync(paymentRequest);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}