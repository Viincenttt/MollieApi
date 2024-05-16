using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IChargebackClient : IBaseMollieClient {
        Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(string? profileId = null, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url);
    }
}
