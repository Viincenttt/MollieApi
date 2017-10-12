using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;

namespace Mollie.Api.Client {
    public class MandateClient : BaseMollieClient, IMandateClient {
        public MandateClient(string apiKey) : base (apiKey) { }

        public async Task<MandateResponse> GetMandateAsync(string customerId, string mandateId) {
            return await this.GetAsync<MandateResponse>($"customers/{customerId}/mandates/{mandateId}").ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, int? offset = null, int? count = null) {
            return await this.GetListAsync<ListResponse<MandateResponse>>($"customers/{customerId}/mandates", offset, count).ConfigureAwait(false);
        }

        public async Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request) {
            return await this.PostAsync<MandateResponse>($"customers/{customerId}/mandates", request).ConfigureAwait(false);
        }

        public async Task RevokeMandate(string customerId, string mandateId) {
            await this.DeleteAsync($"customers/{customerId}/mandates/{mandateId}").ConfigureAwait(false);
        }
    }
}
