using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Refund.Response {
    public record RefundResponseLinks {
        /// <summary>
        /// The API resource URL of the refund itself.
        /// </summary>
        public required UrlObjectLink<RefundResponse> Self { get; init; }

        /// <summary>
        /// The API resource URL of the payment the refund belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; init; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObjectLink<SettlementResponse>? Settlement { get; set; }

        /// <summary>
        /// The URL to the refund retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}
