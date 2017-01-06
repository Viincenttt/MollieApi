using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    using Models.Issuer;
    using Models.List;
    public interface IIssuerClient {
        Task<IssuerResponse> GetIssuerAsync(string issuerId);
        Task<ListResponse<IssuerResponse>> GetIssuerListAsync(int? offset = default(int?), int? count = default(int?));
    }
}
