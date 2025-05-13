using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Shipment.Request;
using Mollie.Api.Models.Shipment.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client
{
    public class ShipmentClient : BaseMollieClient, IShipmentClient {
        public ShipmentClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public ShipmentClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<ShipmentResponse> CreateShipmentAsync(
            string orderId, ShipmentRequest shipmentRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await PostAsync<ShipmentResponse>(
                    $"orders/{orderId}/shipments", shipmentRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ShipmentResponse> GetShipmentAsync(
            string orderId, string shipmentId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            ValidateRequiredUrlParameter(nameof(shipmentId), shipmentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ShipmentResponse>(
                    $"orders/{orderId}/shipments/{shipmentId}{queryParameters.ToQueryString()}",
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ShipmentResponse> GetShipmentAsync(
            UrlObjectLink<ShipmentResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<ShipmentResponse>> GetShipmentListAsync(
            string orderId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ListResponse<ShipmentResponse>>(
                    $"orders/{orderId}/shipments{queryParameters.ToQueryString()}",
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ShipmentResponse>> GetShipmentListAsync(
            UrlObjectLink<ListResponse<ShipmentResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ShipmentResponse> UpdateShipmentAsync(
            string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(orderId), orderId);
            ValidateRequiredUrlParameter(nameof(shipmentId), shipmentId);
            return await PatchAsync<ShipmentResponse>(
                $"orders/{orderId}/shipments/{shipmentId}", shipmentUpdateRequest, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
