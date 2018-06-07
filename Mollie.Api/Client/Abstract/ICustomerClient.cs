using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;

namespace Mollie.Api.Client.Abstract {
    public interface ICustomerClient {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
        Task<CustomerResponse> GetCustomerAsync(string customerId);
        Task<ListResponse<CustomerListData>> GetCustomerListAsync(string from = null, int? limit = null);
    }
}