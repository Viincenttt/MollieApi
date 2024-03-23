using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class RefundClient : BaseMollieClient, IRefundClient {
        public RefundClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            
            if (refundRequest.Testmode.HasValue)
            {
                this.ValidateApiKeyIsOauthAccesstoken();
            }

            return await this.PostAsync<RefundResponse>($"payments/{paymentId}/refunds", refundRequest).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(string? from = null, int? limit = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(testmode: testmode);
            
            return await this.GetListAsync<ListResponse<RefundResponse>>($"refunds", from, limit, queryParameters).ConfigureAwait(false);
        }
        
        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = this.BuildQueryParameters(testmode: testmode);

            return await this.GetListAsync<ListResponse<RefundResponse>>($"payments/{paymentId}/refunds", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url)
        {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetRefundAsync(string paymentId, string refundId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            this.ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = this.BuildQueryParameters(testmode: testmode);
            return await this.GetAsync<RefundResponse>($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task CancelRefundAsync(string paymentId, string refundId, bool testmode = default) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            this.ValidateRequiredUrlParameter(nameof(refundId), refundId);
            var queryParameters = this.BuildQueryParameters(testmode: testmode);
            await this.DeleteAsync($"payments/{paymentId}/refunds/{refundId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            return result;
        }
    }
}