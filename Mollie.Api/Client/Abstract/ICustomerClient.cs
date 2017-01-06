using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    using Models.Customer;
    using Models.List;
    public interface ICustomerClient {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
        Task<CustomerResponse> GetCustomerAsync(string customerId);
        Task<ListResponse<CustomerResponse>> GetCustomerListAsync(int? offset = default(int?), int? count = default(int?));
    }
}
