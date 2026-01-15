using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer.Request;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class CustomerClient : BaseMollieClient, ICustomerClient {
        public CustomerClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public CustomerClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<CustomerResponse> CreateCustomerAsync(
            CustomerRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<CustomerResponse>(
                "customers", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(
            string customerId, CustomerRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<CustomerResponse>(
                $"customers/{customerId}", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteCustomerAsync(
            string customerId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var data = CreateTestmodeModel(testmode);
            await DeleteAsync(
                $"customers/{customerId}", data, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(
            string customerId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<CustomerResponse>(
                $"customers/{customerId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CustomerResponse> GetCustomerAsync(UrlObjectLink<CustomerResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(UrlObjectLink<ListResponse<CustomerResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<CustomerResponse>> GetCustomerListAsync(
            string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetListAsync<ListResponse<CustomerResponse>>(
                    "customers", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetCustomerPaymentListAsync(
            string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<PaymentResponse>>(
                $"customers/{customerId}/payments", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PaymentResponse> CreateCustomerPayment(
            string customerId, PaymentRequest paymentRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<PaymentResponse>(
                $"customers/{customerId}/payments", paymentRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
