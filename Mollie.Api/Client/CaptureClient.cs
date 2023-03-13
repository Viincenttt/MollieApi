using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client
{
    public class CaptureClient : BaseMollieClient, ICaptureClient {
        public CaptureClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            this.ValidateRequiredUrlParameter(nameof(captureId), captureId);
            var queryParameters = BuildQueryParameters(testmode);
            return await this.GetAsync<CaptureResponse>($"payments/{paymentId}/captures/{captureId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCapturesListAsync(string paymentId, bool testmode = false) {
            this.ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await this.GetAsync<ListResponse<CaptureResponse>>($"payments/{paymentId}/captures{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}