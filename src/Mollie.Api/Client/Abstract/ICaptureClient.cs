using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ICaptureClient : IBaseMollieClient {
        Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId, bool testmode = false);
        Task<CaptureResponse> GetCaptureAsync(UrlObjectLink<CaptureResponse> url);
        Task<ListResponse<CaptureResponse>> GetCapturesListAsync(string paymentId, bool testmode = false);
        Task<ListResponse<CaptureResponse>> GetCapturesListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url);
        Task<CaptureResponse> CreateCapture(string paymentId, CaptureRequest captureRequest, bool testmode = false);
    }
}