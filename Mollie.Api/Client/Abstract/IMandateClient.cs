using System.Threading.Tasks;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId);
        Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, string from = null, int? limit = null);
        Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request);
        Task<ListResponse<MandateResponse>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateResponse>> url);
        Task<MandateResponse> GetMandateAsync(UrlObjectLink<MandateResponse> url);
    }
}