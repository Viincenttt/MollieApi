using System;
using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.Capture.Request;

namespace Mollie.Api.Client.Abstract {
    public interface ICaptureClient : IDisposable {
        Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId, bool testmode = false);
        Task<ListResponse<CaptureResponse>> GetCapturesListAsync(string paymentId, bool testmode = false);
        Task<CaptureResponse> CreateCapture(string paymentId, CaptureRequest captureRequest, bool testmode = false);
    }
}