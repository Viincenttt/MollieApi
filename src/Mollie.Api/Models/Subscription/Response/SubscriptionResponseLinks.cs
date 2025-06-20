﻿using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Subscription.Response {
    public record SubscriptionResponseLinks {
        /// <summary>
        ///     The API resource URL of the subscription itself.
        /// </summary>
        public required UrlObjectLink<SubscriptionResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the subscription is for.
        /// </summary>
        public required UrlObjectLink<CustomerResponse> Customer { get; set; }

        /// <summary>
        /// The API resource URL of the payments that are created by this subscription. Not present
        /// if no payments yet created.
        /// </summary>
        public UrlObjectLink<ListResponse<PaymentResponse>>? Payments { get; set; }

        /// <summary>
        /// The API resource URL of the website profile on which this subscription was created.
        /// </summary>
        public required UrlObjectLink<ProfileResponse> Profile { get; set; }

        /// <summary>
        /// The URL to the subscription retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
