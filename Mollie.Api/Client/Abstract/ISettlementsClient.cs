using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Settlement;

namespace Mollie.Api.Client.Abstract {
    public interface ISettlementsClient {
        Task<SettlementResponse> GetSettlementAsync(string settlementId);
        Task<SettlementResponse> GetNextSettlement();
        Task<SettlementResponse> GetOpenBalance();
        Task<ListResponse<SettlementListData>> GetSettlementsListAsync(string reference = null, string from = null, int? limit = null);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(string settlementId, string from = null, int? limit = null);
        Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(string settlementId, string from = null, int? limit = null);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(string settlementId, string from = null, int? limit = null);
    }
}