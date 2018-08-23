using System.Threading.Tasks;
using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Overview;

namespace Mollie.WebApplicationCoreExample.Services.Customer {
    public class CustomerOverviewClient : OverviewClientBase<CustomerResponse>, ICustomerOverviewClient {
        private readonly ICustomerClient _customerClient;

        public CustomerOverviewClient(IMapper mapper, ICustomerClient customerClient) : base(mapper) {
            this._customerClient = customerClient;
        }

        public async Task<OverviewModel<CustomerResponse>> GetList() {
            return this.Map(await this._customerClient.GetCustomerListAsync());
        }

        public async Task<OverviewModel<CustomerResponse>> GetListByUrl(string url) {
            return this.Map(await this._customerClient.GetCustomerListAsync(this.CreateUrlObject(url)));
        }
    }
}