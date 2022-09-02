using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Capture;

namespace Mollie.Api.Client.Abstract {
    public interface ICaptureClient {
        Task<CaptureResponse> GetCaptureAsync(string paymentId, string captureId, bool testmode = false);
        Task<ListResponse<CaptureResponse>> GetCapturesListAsync(string paymentId, bool testmode = false);
    }
}