using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public class ProfileResponseLinks {
        public UrlObjectLink<ProfileResponse> Self { get; set; }
        public UrlObjectLink<ListResponse<ChargebackListData>> Chargebacks { get; set; }
        public UrlObjectLink<ListResponse<PaymentMethodListData>> Methods { get; set; }
        public UrlObjectLink<ListResponse<PaymentMethodListData>> Payments { get; set; }
        public UrlObjectLink<ListResponse<RefundListData>> Refunds { get; set; }
        public UrlLink CheckoutPreviewUrl { get; set; }
        public UrlLink Documentation { get; set; }
    }
}