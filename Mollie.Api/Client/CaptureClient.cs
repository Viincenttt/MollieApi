using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client
{
    public class CaptureClient : BaseMollieClient, ICaptureClient {
        public CaptureClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId) {
            return await this.GetAsync<CaptureResponse>($"payments/{paymentId}/captures/{captureId}").ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetCapturesListAsync(string paymentId) {
            return await this.GetAsync<ListResponse<CaptureResponse>>($"payments/{paymentId}/captures").ConfigureAwait(false);
        }
    }
}