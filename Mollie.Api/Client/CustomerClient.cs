using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class CustomerClient : BaseMollieClient, ICustomerClient {
        public CustomerClient(string apiKey) : base(apiKey) {
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request) {
            return await this.PostAsync<CustomerResponse>($"customers", request).ConfigureAwait(false);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest request) {
            return await this.PostAsync<CustomerResponse>($"customers/{customerId}", request).ConfigureAwait(false);
        }

        public async Task DeleteCustomerAsync(string customerId) {
            await this.DeleteAsync($"customers/{customerId}").ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(string customerId) {
            return await this.GetAsync<CustomerResponse>($"customers/{customerId}").ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(UrlObjectLink<CustomerResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(UrlObjectLink<ListResponse<CustomerResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(string from = null, int? limit = null) {
            return await this.GetListAsync<ListResponse<CustomerResponse>>("customers", from, limit)
                .ConfigureAwait(false);
        }
    }
}