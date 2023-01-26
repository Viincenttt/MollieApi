using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class OrderClient : BaseMollieClient, IOrderClient {
        public OrderClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest) {
            return await this.PostAsync<OrderResponse>("orders", orderRequest).ConfigureAwait(false);
        }

        public async Task<OrderResponse> GetOrderAsync(string orderId, bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(embedPayments, embedRefunds, embedShipments, testmode);
            return await this.GetAsync<OrderResponse>($"orders/{orderId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<OrderResponse> UpdateOrderAsync(string orderId, OrderUpdateRequest orderUpdateRequest) {
            return await this.PatchAsync<OrderResponse>($"orders/{orderId}", orderUpdateRequest).ConfigureAwait(false); 
        }

        public async Task<OrderResponse> UpdateOrderLinesAsync(string orderId, string orderLineId, OrderLineUpdateRequest orderLineUpdateRequest) {
            return await this.PatchAsync<OrderResponse>($"orders/{orderId}/lines/{orderLineId}", orderLineUpdateRequest).ConfigureAwait(false);
        }

        public async Task<OrderResponse> ManageOrderLinesAsync(string orderId, ManageOrderLinesRequest manageOrderLinesRequest) {
            return await this.PatchAsync<OrderResponse>($"orders/{orderId}/lines", manageOrderLinesRequest).ConfigureAwait(false);
        }

        public async Task CancelOrderAsync(string orderId, bool testmode = false) {
            var data = TestmodeModel.Create(testmode);
            await this.DeleteAsync($"orders/{orderId}", data).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrderResponse>> GetOrderListAsync(string from = null, int? limit = null, string profileId = null, bool testmode = false) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await this.GetListAsync<ListResponse<OrderResponse>>($"orders", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<OrderResponse>> GetOrderListAsync(UrlObjectLink<ListResponse<OrderResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task CancelOrderLinesAsync(string orderId, OrderLineCancellationRequest cancelationRequest) {
            await this.DeleteAsync($"orders/{orderId}/lines", cancelationRequest).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> CreateOrderPaymentAsync(string orderId, OrderPaymentRequest createOrderPaymentRequest) {
            return await this.PostAsync<PaymentResponse>($"orders/{orderId}/payments", createOrderPaymentRequest).ConfigureAwait(false);
        }

        public async Task<OrderRefundResponse> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest) {
            return await this.PostAsync<OrderRefundResponse>($"orders/{orderId}/refunds", createOrderRefundRequest);
        }

        public async Task<ListResponse<RefundResponse>> GetOrderRefundListAsync(string orderId, string from = null, int? limit = null, bool testmode = false) {
            var queryParameters = BuildQueryParameters(null, testmode);
            return await this.GetListAsync<ListResponse<RefundResponse>>($"orders/{orderId}/refunds", from, limit, queryParameters).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(string profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false, bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("embed", this.BuildEmbedParameter(embedPayments, embedRefunds, embedShipments));
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }

        private string BuildEmbedParameter(bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("payments", embedPayments);
            includeList.AddValueIfTrue("refunds", embedRefunds);
            includeList.AddValueIfTrue("shipments", embedShipments);
            return includeList.ToIncludeParameter();
        }
    }
}