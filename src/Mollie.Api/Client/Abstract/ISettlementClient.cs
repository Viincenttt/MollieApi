using System.Collections.Generic;
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
        /// <summary>
        /// Retrieve a settlement by its ID.
        /// </summary>
        /// <param name="settlementId">The settlement's ID, for example stl_jDk30akdN.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The settlement object.</returns>
        Task<SettlementResponse> GetSettlementAsync(string settlementId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the details of the current settlement that has not yet been paid out.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The next settlement object.</returns>
        Task<SettlementResponse> GetNextSettlementAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the details of the open settlement that is not yet finalized.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The open settlement object.</returns>
        Task<SettlementResponse> GetOpenSettlementAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a list of settlements, optionally filtered by balance, year, month, or currency.
        /// </summary>
        /// <param name="balanceId">The balance ID to filter by.</param>
        /// <param name="year">The year to filter by.</param>
        /// <param name="month">The month to filter by.</param>
        /// <param name="currencies">The currencies to filter by.</param>
        /// <param name="from">The settlement ID to start from (pagination).</param>
        /// <param name="limit">The maximum number of settlements to return.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of settlement objects.</returns>
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(string? balanceId = null, int? year = null, int? month = null, IEnumerable<string>? currencies = null, string? from = null, int? limit = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a list of settlements using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the next page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of settlement objects.</returns>
        Task<ListResponse<SettlementResponse>> GetSettlementListAsync(UrlObjectLink<ListResponse<SettlementResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of payments included in a settlement.
        /// </summary>
        /// <param name="settlementId">The settlement's ID.</param>
        /// <param name="from">The payment ID to start from (pagination).</param>
        /// <param name="limit">The maximum number of payments to return.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of payment objects.</returns>
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of payments included in a settlement using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the next page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of payment objects.</returns>
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of refunds included in a settlement.
        /// </summary>
        /// <param name="settlementId">The settlement's ID.</param>
        /// <param name="from">The refund ID to start from (pagination).</param>
        /// <param name="limit">The maximum number of refunds to return.</param>
        /// <param name="embedPayment">Set to true to embed the full payment object in the refund response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of refund objects.</returns>
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(string settlementId, string? from = null, int? limit = null, bool embedPayment = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of refunds included in a settlement using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the next page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of refund objects.</returns>
        Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of chargebacks included in a settlement.
        /// </summary>
        /// <param name="settlementId">The settlement's ID.</param>
        /// <param name="from">The chargeback ID to start from (pagination).</param>
        /// <param name="limit">The maximum number of chargebacks to return.</param>
        /// <param name="embedPayment">Set to true to embed the full payment object in the chargeback response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of chargeback objects.</returns>
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(string settlementId, string? from = null, int? limit = null, bool embedPayment = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of chargebacks included in a settlement using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the next page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of chargeback objects.</returns>
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of captures included in a settlement.
        /// </summary>
        /// <param name="settlementId">The settlement's ID.</param>
        /// <param name="from">The capture ID to start from (pagination).</param>
        /// <param name="limit">The maximum number of captures to return.</param>
        /// <param name="embedPayment">Set to true to embed the full payment object in the capture response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of capture objects.</returns>
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(string settlementId, string? from = null, int? limit = null, bool embedPayment = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the list of captures included in a settlement using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the next page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of capture objects.</returns>
        Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a settlement using a URL object link.
        /// </summary>
        /// <param name="url">The URL object link to the settlement.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The settlement object.</returns>
        Task<SettlementResponse> GetSettlementAsync(UrlObjectLink<SettlementResponse> url, CancellationToken cancellationToken = default);
    }
}
