using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISettlementsClient : IBaseMollieClient {
        Task<SettlementResponse> GetSettlementAsync(string settlementId);
        Task<SettlementResponse> GetNextSettlement();
        Task<SettlementResponse> GetOpenSettlement();
        Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(string reference = null, string from = null, int? limit = null);
        Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(UrlObjectLink<ListResponse<SettlementResponse>> url);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(string settlementId, string from = null, int? limit = null);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url);
        Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(string settlementId, string from = null, int? limit = null);
        Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(UrlObjectLink<ListResponse<RefundResponse>> url);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(string settlementId, string from = null, int? limit = null);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url);
        Task<ListResponse<CaptureResponse>> GetSettlementCapturesListAsync(string settlementId, string offset = null, int? count = null);
        Task<ListResponse<CaptureResponse>> GetSettlementCapturesListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url);
        Task<SettlementResponse> GetSettlementAsync(UrlObjectLink<SettlementResponse> url);
        
        [Obsolete("Use GetSettlementCapturesListAsync instead")]
        Task<ListResponse<CaptureResponse>> ListSettlementCapturesAsync(string settlementId);
    }
}
