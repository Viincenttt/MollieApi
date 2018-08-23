using System.Threading.Tasks;
using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Customer {
    public class CustomerStorageClient : ICustomerStorageClient {
        private readonly ICustomerClient _customerClient;
        private readonly IMapper _mapper;

        public CustomerStorageClient(ICustomerClient customerClient, IMapper mapper) {
            this._customerClient = customerClient;
            this._mapper = mapper;
        }

        public async Task Create(CreateCustomerModel model) {
            CustomerRequest customerRequest = this._mapper.Map<CustomerRequest>(model);
            await this._customerClient.CreateCustomerAsync(customerRequest);
        }
    }
}