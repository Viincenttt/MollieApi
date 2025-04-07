using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Request;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient : IBaseMollieClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request, CancellationToken cancellationToken = default);
        Task<ListResponse<MandateResponse>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateResponse>> url, CancellationToken cancellationToken = default);
        Task<MandateResponse> GetMandateAsync(UrlObjectLink<MandateResponse> url, CancellationToken cancellationToken = default);
        Task RevokeMandate(string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default);
    }
}
