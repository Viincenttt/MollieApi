using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class CustomerClient : BaseMollieClient, ICustomerClient {
        public CustomerClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request) {
            return await this.PostAsync<CustomerResponse>($"customers", request).ConfigureAwait(false);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest request) {
            this.ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await this.PostAsync<CustomerResponse>($"customers/{customerId}", request).ConfigureAwait(false);
        }

        public async Task DeleteCustomerAsync(string customerId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var data = TestmodeModel.Create(testmode);
            await this.DeleteAsync($"customers/{customerId}", data).ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(string customerId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetAsync<CustomerResponse>($"customers/{customerId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(UrlObjectLink<CustomerResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(UrlObjectLink<ListResponse<CustomerResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(string? from = null, int? limit = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetListAsync<ListResponse<CustomerResponse>>("customers", from, limit, queryParameters)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetCustomerPaymentListAsync(string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await this.GetListAsync<ListResponse<PaymentResponse>>($"customers/{customerId}/payments", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> CreateCustomerPayment(string customerId, PaymentRequest paymentRequest) {
            this.ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<PaymentResponse>($"customers/{customerId}/payments", paymentRequest).ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
        
        private Dictionary<string, string> BuildQueryParameters(string? profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}