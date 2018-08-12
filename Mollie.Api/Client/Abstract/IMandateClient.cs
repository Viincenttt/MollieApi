using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId);
        Task<ListResponse<MandateListData>> GetMandateListAsync(string customerId, string from = null, int? limit = null);
        Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request);
        Task<ListResponse<MandateListData>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateListData>> url);
        Task<MandateResponse> GetMandateAsync(UrlObjectLink<MandateResponse> url);
    }
}