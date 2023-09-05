using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Shipment;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client
{
    public class ShipmentClient : BaseMollieClient, IShipmentClient {
        public ShipmentClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ShipmentResponse> CreateShipmentAsync(string orderId, ShipmentRequest shipmentRequest) {
            this.ValidateRequiredUrlParameter(nameof(orderId), orderId);
            return await this.PostAsync<ShipmentResponse>($"orders/{orderId}/shipments", shipmentRequest)
                .ConfigureAwait(false);
        }

        public async Task<ShipmentResponse> GetShipmentAsync(string orderId, string shipmentId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(orderId), orderId);
            this.ValidateRequiredUrlParameter(nameof(shipmentId), shipmentId);
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetAsync<ShipmentResponse>($"orders/{orderId}/shipments/{shipmentId}{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }
        
        public async Task<ShipmentResponse> GetShipmentAsync(UrlObjectLink<ShipmentResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(string orderId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(orderId), orderId);
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetAsync<ListResponse<ShipmentResponse>>($"orders/{orderId}/shipments{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }
        
        public async Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(UrlObjectLink<ListResponse<ShipmentResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ShipmentResponse> UpdateShipmentAsync(string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest) {
            this.ValidateRequiredUrlParameter(nameof(orderId), orderId);
            this.ValidateRequiredUrlParameter(nameof(shipmentId), shipmentId);
            return await this.PatchAsync<ShipmentResponse>($"orders/{orderId}/shipments/{shipmentId}", shipmentUpdateRequest).ConfigureAwait(false); 
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}