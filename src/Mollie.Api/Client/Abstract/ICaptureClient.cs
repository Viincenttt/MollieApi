using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using System.Threading;

namespace Mollie.Api.Client.Abstract {
    public interface ICaptureClient : IBaseMollieClient {
        Task<MollieResult<CaptureResponse>> GetCaptureAsync(string paymentId, string captureId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<CaptureResponse>> GetCaptureAsync(UrlObjectLink<CaptureResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<CaptureResponse>>> GetCaptureListAsync(string paymentId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<CaptureResponse>>> GetCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url, CancellationToken cancellationToken = default);
        Task<MollieResult<CaptureResponse>> CreateCapture(string paymentId, CaptureRequest captureRequest, CancellationToken cancellationToken = default);
    }
}
