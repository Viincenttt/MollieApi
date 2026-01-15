using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Refund.Request;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class RefundClient : BaseMollieClient, IRefundClient {
        public RefundClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public RefundClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<RefundResponse> CreatePaymentRefundAsync(
            string paymentId, RefundRequest refundRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            if (refundRequest.Testmode.HasValue)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            return await PostAsync<RefundResponse>(
                $"payments/{paymentId}/refunds", refundRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(
            string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<RefundResponse>>(
                "refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetPaymentRefundListAsync(
            string paymentId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<RefundResponse>>(
                $"payments/{paymentId}/refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default)
        {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetPaymentRefundAsync(
            string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<RefundResponse>(
                $"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CancelPaymentRefundAsync(
            string paymentId, string refundId, bool testmode = default, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            await DeleteAsync($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<OrderRefundResponse> CreateOrderRefundAsync(
            string orderId, OrderRefundRequest createOrderRefundRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PostAsync<OrderRefundResponse>(
                $"orders/{orderId}/refunds", createOrderRefundRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetOrderRefundListAsync(
            string orderId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetListAsync<ListResponse<RefundResponse>>(
                $"orders/{orderId}/refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
