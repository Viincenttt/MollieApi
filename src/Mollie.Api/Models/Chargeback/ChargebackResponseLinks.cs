using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Chargeback {
    public record ChargebackResponseLinks {
        /// <summary>
        /// The API resource URL of the chargeback itself.
        /// </summary>
        public required UrlObjectLink<ChargebackResponse> Self { get; init; }

        /// <summary>
        /// The API resource URL of the payment this chargeback belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; init; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObjectLink<SettlementResponse>? Settlement { get; set; }

        /// <summary>
        /// The URL to the chargeback retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}