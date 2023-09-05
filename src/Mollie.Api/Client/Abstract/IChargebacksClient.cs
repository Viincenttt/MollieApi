using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IChargebacksClient : IDisposable {
        Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string paymentId, string from = null, int? limit = null, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string profileId = null, bool testmode = false);
        Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url);
    }
}