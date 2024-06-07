using System.Threading.Tasks;
using Mollie.Api.Models.Customer.Request;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ICustomerClient : IBaseMollieClient {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
        Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest request);
        Task DeleteCustomerAsync(string customerId, bool testmode = false);
        Task<CustomerResponse> GetCustomerAsync(string customerId, bool testmode = false);
        Task<CustomerResponse> GetCustomerAsync(UrlObjectLink<CustomerResponse> url);
        Task<ListResponse<CustomerResponse>> GetCustomerListAsync(UrlObjectLink<ListResponse<CustomerResponse>> url);
        Task<ListResponse<CustomerResponse>> GetCustomerListAsync(string? from = null, int? limit = null, bool testmode = false);
        Task<ListResponse<PaymentResponse>> GetCustomerPaymentListAsync(string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false);
        Task<PaymentResponse> CreateCustomerPayment(string customerId, PaymentRequest paymentRequest);
    }
}
