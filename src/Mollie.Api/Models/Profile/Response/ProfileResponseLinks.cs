﻿using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Profile.Response {
    public class ProfileResponseLinks {
        public UrlObjectLink<ProfileResponse> Self { get; set; }
        public UrlLink Dashboard { get; set; }
        public UrlObjectLink<ListResponse<ChargebackResponse>> Chargebacks { get; set; }
        public UrlObjectLink<ListResponse<PaymentResponse>> Methods { get; set; }
        public UrlObjectLink<ListResponse<PaymentMethodResponse>> Payments { get; set; }
        public UrlObjectLink<ListResponse<RefundResponse>> Refunds { get; set; }
        public UrlLink CheckoutPreviewUrl { get; set; }
        public UrlLink Documentation { get; set; }
    }
}