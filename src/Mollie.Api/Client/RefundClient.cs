using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Refund.Request;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class RefundClient : BaseMollieClient, IRefundClient {
        public RefundClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public RefundClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
        }

        public async Task<RefundResponse> CreatePaymentRefundAsync(string paymentId, RefundRequest refundRequest) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            if (refundRequest.Testmode.HasValue)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            return await PostAsync<RefundResponse>($"payments/{paymentId}/refunds", refundRequest).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(string? from = null, int? limit = null, bool testmode = false) {
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<RefundResponse>>($"refunds", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetPaymentRefundListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode: testmode);

            return await GetListAsync<ListResponse<RefundResponse>>($"payments/{paymentId}/refunds", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetPaymentRefundAsync(string paymentId, string refundId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<RefundResponse>($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task CancelPaymentRefundAsync(string paymentId, string refundId, bool testmode = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            await DeleteAsync($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<OrderRefundResponse> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PostAsync<OrderRefundResponse>($"orders/{orderId}/refunds", createOrderRefundRequest);
        }

        public async Task<ListResponse<RefundResponse>> GetOrderRefundListAsync(string orderId, string? from = null, int? limit = null, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetListAsync<ListResponse<RefundResponse>>($"orders/{orderId}/refunds", from, limit, queryParameters).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            return result;
        }
    }
}
