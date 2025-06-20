using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Settlement.Response {
    public record SettlementResponseLinks {
        /// <summary>
        /// The API resource URL of the settlement itself.
        /// </summary>
        public required UrlObjectLink<SettlementResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the payments that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<PaymentResponse>> Payments { get; set; }

        /// <summary>
        /// The API resource URL of the refunds that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<RefundResponse>> Refunds { get; set; }

        /// <summary>
        /// The API resource URL of the chargebacks that are included in this settlement.
        /// </summary>
        public required UrlObjectLink<ListResponse<ChargebackResponse>> Chargebacks { get; set; }

        /// <summary>
        /// The URL to the settlement retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
