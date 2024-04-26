using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Settlement {
    public record SettlementResponseLinks {
        /// <summary>
        /// The API resource URL of the settlement itself.
        /// </summary>
        public required UrlObjectLink<SettlementResponse> Self { get; init; }

        /// <summary>
        /// The API resource URL of the payments that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<PaymentResponse>> Payments { get; init; }

        /// <summary>
        /// The API resource URL of the refunds that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<RefundResponse>> Refunds { get; init; }

        /// <summary>
        /// The API resource URL of the chargebacks that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<ChargebackResponse>> Chargebacks { get; init; }

        /// <summary>
        /// The URL to the settlement retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}