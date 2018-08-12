using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;

namespace Mollie.Api.Client.Abstract {
    public interface IChargebacksClient {
        Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId);
        Task<ListResponse<ChargebackListData>> GetChargebacksListAsync(string paymentId, string from = null, int? limit = null);
        Task<ListResponse<ChargebackListData>> GetChargebacksListAsync(string oathProfileId = null, bool? oauthTestmode = null);
    }
}