using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;

namespace Mollie.Api.Client.Abstract {
    public interface IMandateClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId);

        Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, int? offset = default(int?),
            int? count = default(int?));

        Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request);
    }
}