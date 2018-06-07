using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId);

        Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, string from = null, int? limit = null);

        Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request);
    }
}