using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class OrderClient : BaseMollieClient, IOrderClient {
        public OrderClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public OrderClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<OrderResponse> CreateOrderAsync(

            OrderRequest orderRequest, CancellationToken cancellationToken = default) {
            return await PostAsync<OrderResponse>("orders", orderRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<OrderResponse> GetOrderAsync(
            string orderId, bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = BuildQueryParameters(embedPayments, embedRefunds, embedShipments, testmode);
            return await GetAsync<OrderResponse>(
                $"orders/{orderId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<OrderResponse> GetOrderAsync(
            UrlObjectLink<OrderResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<OrderResponse> UpdateOrderAsync(
            string orderId, OrderUpdateRequest orderUpdateRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PatchAsync<OrderResponse>(
                $"orders/{orderId}", orderUpdateRequest, cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<OrderResponse> UpdateOrderLinesAsync(
            string orderId, string orderLineId, OrderLineUpdateRequest orderLineUpdateRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            ValidateRequiredUrlParameter(nameof(orderLineId), orderLineId);
            return await PatchAsync<OrderResponse>(
                $"orders/{orderId}/lines/{orderLineId}", orderLineUpdateRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<OrderResponse> ManageOrderLinesAsync(
            string orderId, ManageOrderLinesRequest manageOrderLinesRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PatchAsync<OrderResponse>(
                $"orders/{orderId}/lines", manageOrderLinesRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CancelOrderAsync(
            string orderId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var data = TestmodeModel.Create(testmode);
            await DeleteAsync(
                $"orders/{orderId}", data, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<OrderResponse>> GetOrderListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false, SortDirection? sort = null, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId, testmode, sort);
            return await GetListAsync<ListResponse<OrderResponse>>(
                "orders", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<OrderResponse>> GetOrderListAsync(
            UrlObjectLink<ListResponse<OrderResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CancelOrderLinesAsync(
            string orderId, OrderLineCancellationRequest cancelationRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            await DeleteAsync($"orders/{orderId}/lines", cancelationRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PaymentResponse> CreateOrderPaymentAsync(
            string orderId, OrderPaymentRequest createOrderPaymentRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PostAsync<PaymentResponse>(
                $"orders/{orderId}/payments", createOrderPaymentRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(string? profileId = null, bool testmode = false, SortDirection? sort = null) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            result.AddValueIfTrue(nameof(testmode), testmode);
            result.AddValueIfNotNullOrEmpty(nameof(sort), sort?.ToString()?.ToLowerInvariant());
            return result;
        }

        private Dictionary<string, string> BuildQueryParameters(bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false, bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("embed", BuildEmbedParameter(embedPayments, embedRefunds, embedShipments));
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
