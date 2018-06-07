using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client {
    public class IssuerClient : BaseMollieClient, IIssuerClient {
        public IssuerClient(string apiKey) : base(apiKey) {
        }

        public async Task<ListResponse<IssuerResponse>> GetIssuerListAsync(string from = null, int? limit = null) {
            return await this.GetListAsync<ListResponse<IssuerResponse>>("issuers", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<IssuerResponse> GetIssuerAsync(string issuerId) {
            return await this.GetAsync<IssuerResponse>($"issuers/{issuerId}").ConfigureAwait(false);
        }
    }
}