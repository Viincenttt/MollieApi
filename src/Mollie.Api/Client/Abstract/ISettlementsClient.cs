using System.Threading.Tasks;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISettlementsClient : IBaseMollieClient {
        Task<SettlementResponse> GetSettlementAsync(string settlementId);
        Task<SettlementResponse> GetNextSettlement();
        Task<SettlementResponse> GetOpenSettlement();
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(string? reference = null, string? from = null, int? limit = null);
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(UrlObjectLink<ListResponse<SettlementResponse>> url);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(string settlementId, string? from = null, int? limit = null);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url);
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(string settlementId, string? from = null, int? limit = null);
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(string settlementId, string? from = null, int? limit = null);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url);
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(string settlementId, string? offset = null, int? count = null);
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url);
        Task<SettlementResponse> GetSettlementAsync(UrlObjectLink<SettlementResponse> url);
    }
}
