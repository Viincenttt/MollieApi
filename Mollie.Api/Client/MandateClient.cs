using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class MandateClient : BaseMollieClient, IMandateClient {
        public MandateClient(string apiKey) : base(apiKey) {
        }

        public async Task<MandateResponse> GetMandateAsync(string customerId, string mandateId) {
            return await this.GetAsync<MandateResponse>($"customers/{customerId}/mandates/{mandateId}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateListData>> GetMandateListAsync(string customerId, string from = null, int? limit = null) {
            return await this
                .GetListAsync<ListResponse<MandateListData>>($"customers/{customerId}/mandates", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request) {
            return await this.PostAsync<MandateResponse>($"customers/{customerId}/mandates", request)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateListData>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateListData>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<MandateResponse> GetMandateAsync(UrlObjectLink<MandateResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task RevokeMandate(string customerId, string mandateId) {
            await this.DeleteAsync($"customers/{customerId}/mandates/{mandateId}").ConfigureAwait(false);
        }
    }
}