using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class PaymentController : Controller {
        private readonly IPaymentClient _paymentClient;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentClient paymentClient, IMapper mapper) {
            this._paymentClient = paymentClient;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            ListResponse<PaymentResponse> paymentList = await this._paymentClient.GetPaymentListAsync();
            OverviewModel<PaymentResponse> model = this._mapper.Map<OverviewModel<PaymentResponse>>(paymentList);

            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> Next([FromQuery]string url) {
            return await this.GetListByUrl(url);
        }

        [HttpGet]
        public async Task<ViewResult> Previous([FromQuery] string url) {
            return await this.GetListByUrl(url);
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

        private async Task<ViewResult> GetListByUrl(string url) {
            UrlObjectLink<ListResponse<PaymentResponse>> urlObject = new UrlObjectLink<ListResponse<PaymentResponse>>() {
                Href = url
            };

            ListResponse<PaymentResponse> paymentList = await this._paymentClient.GetPaymentListAsync(urlObject);
            OverviewModel<PaymentResponse> model = this._mapper.Map<OverviewModel<PaymentResponse>>(paymentList);
            return this.View(nameof(this.Index), model);
        }
    }
}