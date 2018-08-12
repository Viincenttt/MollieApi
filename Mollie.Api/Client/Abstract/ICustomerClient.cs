using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ICustomerClient {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
        Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest request);
        Task DeleteCustomerAsync(string customerId);
        Task<CustomerResponse> GetCustomerAsync(string customerId);
        Task<CustomerResponse> GetCustomerAsync(UrlLink url);
        Task<ListResponse<CustomerListData>> GetCustomerListAsync(string from = null, int? limit = null);
    }
}