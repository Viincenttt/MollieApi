using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Request;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient : IBaseMollieClient {
        Task<MollieResult<MandateResponse>> GetMandateAsync(string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<MandateResponse>>> GetMandateListAsync(string customerId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<MandateResponse>> CreateMandateAsync(string customerId, MandateRequest request, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<MandateResponse>>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateResponse>> url, CancellationToken cancellationToken = default);
        Task<MollieResult<MandateResponse>> GetMandateAsync(UrlObjectLink<MandateResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult> RevokeMandate(string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default);
    }
}
