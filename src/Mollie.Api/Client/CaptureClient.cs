using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client
{
    public class CaptureClient : BaseMollieClient, ICaptureClient {
        public CaptureClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(captureId), captureId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<CaptureResponse>($"payments/{paymentId}/captures/{captureId}{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }

        public async Task<CaptureResponse> GetCaptureAsync(UrlObjectLink<CaptureResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCaptureListAsync(string paymentId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ListResponse<CaptureResponse>>($"payments/{paymentId}/captures{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<CaptureResponse> CreateCapture(string paymentId, CaptureRequest captureRequest, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await PostAsync<CaptureResponse>($"payments/{paymentId}/captures{queryParameters.ToQueryString()}", captureRequest)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
