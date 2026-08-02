using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
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

        public async Task<MollieResult<PaymentRefundResponse>> CreatePaymentRefundAsync(
            string paymentId, PaymentRefundRequest paymentRefundRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            if (paymentRefundRequest.Testmode.HasValue)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            return await PostAsync<PaymentRefundResponse>(
                $"payments/{paymentId}/refunds", paymentRefundRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<PaymentRefundResponse>>> GetRefundListAsync(
            string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<PaymentRefundResponse>>(
                "refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<PaymentRefundResponse>>> GetPaymentRefundListAsync(
            string paymentId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<PaymentRefundResponse>>(
                $"payments/{paymentId}/refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<PaymentRefundResponse>>> GetPaymentRefundListAsync(UrlObjectLink<ListResponse<PaymentRefundResponse>> url, CancellationToken cancellationToken = default)
        {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MollieResult<PaymentRefundResponse>> GetPaymentRefundAsync(UrlObjectLink<PaymentRefundResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MollieResult<PaymentRefundResponse>> GetPaymentRefundAsync(
            string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<PaymentRefundResponse>(
                $"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> CancelPaymentRefundAsync(
            string paymentId, string refundId, bool testmode = default, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await DeleteAsync($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<OrderRefundResponse>> CreateOrderRefundAsync(
            string orderId, OrderRefundRequest createOrderRefundRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PostAsync<OrderRefundResponse>(
                $"orders/{orderId}/refunds", createOrderRefundRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<OrderRefundResponse>>> GetOrderRefundListAsync(
            string orderId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetListAsync<ListResponse<OrderRefundResponse>>(
                $"orders/{orderId}/refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
