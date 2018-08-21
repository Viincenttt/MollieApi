using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class CustomerController : Controller {
        private readonly ICustomerClient _customerClient;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerClient customerClient, IMapper mapper) {
            this._customerClient = customerClient;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            ListResponse<CustomerResponse> customerList = await this._customerClient.GetCustomerListAsync();
            OverviewModel<CustomerResponse> model = this._mapper.Map<OverviewModel<CustomerResponse>>(customerList);

            return this.View(model);
        }
    }
}