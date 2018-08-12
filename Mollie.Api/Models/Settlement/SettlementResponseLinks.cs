using System.Collections.Generic;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Settlement {
    public class SettlementResponseLinks {
        /// <summary>
        /// The API resource URL of the settlement itself.
        /// </summary>
        public UrlObjectLink<SettlementResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the payments that are included in this settlement.
        /// </summary>
        public UrlObjectLink<IEnumerable<PaymentResponse>> Payments { get; set; }

        /// <summary>
        /// The API resource URL of the refunds that are included in this settlement.
        /// </summary>
        public UrlObjectLink<IEnumerable<RefundResponse>> Refunds { get; set; }

        /// <summary>
        /// The API resource URL of the chargebacks that are included in this settlement.
        /// </summary>
        public UrlObjectLink<IEnumerable<ChargebackResponse>> Chargebacks { get; set; }

        /// <summary>
        /// The URL to the settlement retrieval endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
    }
}