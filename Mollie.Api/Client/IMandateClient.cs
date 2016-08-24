using System.Threading.Tasks;

namespace Mollie.Api.Client {
    using Models.Mandate;
    using Models.List;
    public interface IMandateClient {
        Task<MandateResponse> GetMandateAsync(string customerId, string mandateId);
        Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, int? offset = default(int?), int? count = default(int?));
    }
}
