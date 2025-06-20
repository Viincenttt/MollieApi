using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public record ProfileResponseLinks {
        public required UrlObjectLink<ProfileResponse> Self { get; set; }
        public required UrlLink Dashboard { get; set; }
        public UrlObjectLink<ListResponse<ChargebackResponse>>? Chargebacks { get; set; }
        public UrlObjectLink<ListResponse<PaymentResponse>>? Methods { get; set; }
        public UrlObjectLink<ListResponse<PaymentMethodResponse>>? Payments { get; set; }
        public UrlObjectLink<ListResponse<RefundResponse>>? Refunds { get; set; }
        public UrlLink? CheckoutPreviewUrl { get; set; }
        public UrlLink? Documentation { get; set; }
    }
}
