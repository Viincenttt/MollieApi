using System.Collections.Generic;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public class ProfileResponseLinks {
        public UrlObjectLink<ProfileResponse> Self { get; set; }
        public UrlObjectLink<IEnumerable<ChargebackResponse>> Chargebacks { get; set; }
        public UrlObjectLink<IEnumerable<PaymentMethodResponse>> Methods { get; set; }
        public UrlObjectLink<IEnumerable<PaymentMethodResponse>> Payments { get; set; }
        public UrlObjectLink<IEnumerable<RefundResponse>> Refunds { get; set; }
        public UrlLink CheckoutPreviewUrl { get; set; }
        public UrlLink Documentation { get; set; }
    }
}