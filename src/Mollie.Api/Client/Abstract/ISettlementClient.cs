using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISettlementClient : IBaseMollieClient {
        Task<SettlementResponse> GetSettlementAsync(string settlementId, CancellationToken cancellationToken = default);
        Task<SettlementResponse> GetNextSettlement(CancellationToken cancellationToken = default);
        Task<SettlementResponse> GetOpenSettlement(CancellationToken cancellationToken = default);
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(string? reference = null, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(UrlObjectLink<ListResponse<SettlementResponse>> url, CancellationToken cancellationToken = default);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url, CancellationToken cancellationToken = default);
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url, CancellationToken cancellationToken = default);
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(string settlementId, string? offset = null, int? count = null, CancellationToken cancellationToken = default);
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url, CancellationToken cancellationToken = default);
        Task<SettlementResponse> GetSettlementAsync(UrlObjectLink<SettlementResponse> url, CancellationToken cancellationToken = default);
    }
}
