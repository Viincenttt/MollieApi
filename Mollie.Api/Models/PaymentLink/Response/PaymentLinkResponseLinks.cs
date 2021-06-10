using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Subscription;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.PaymentLink.Response {
    public class PaymentLinkResponseLinks {
        /// <summary>
        /// The API resource URL of the payment link itself.
        /// </summary>
        public UrlObjectLink<PaymentLinkResponse> Self { get; set; }

        /// <summary>
        /// Direct link to the payment link.
        /// </summary>
        public UrlLink PaymentLink { get; set; } 

        /// <summary>
        ///The URL to the payment link retrieval endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
          
    }
}