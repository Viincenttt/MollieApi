using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface ICustomerClient {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
        Task<CustomerResponse> GetCustomerAsync(string customerId);

        Task<ListResponse<CustomerResponse>> GetCustomerListAsync(int? offset = default(int?),
            int? count = default(int?));
    }
}