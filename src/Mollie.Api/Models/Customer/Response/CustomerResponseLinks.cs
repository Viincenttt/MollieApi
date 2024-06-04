﻿using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Customer.Response {
    public record CustomerResponseLinks {
        /// <summary>
        /// The API resource URL of the customer itself.
        /// </summary>
        public required UrlObjectLink<CustomerResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the subscriptions belonging to the Customer, if there are no subscriptions this parameter is omitted.
        /// </summary>
        public UrlObjectLink<ListResponse<SubscriptionResponse>>? Subscriptions { get; set; }

        /// <summary>
        /// The API resource URL of the payments belonging to the Customer, if there are no payments this parameter is omitted.
        /// </summary>
        public UrlObjectLink<ListResponse<PaymentResponse>>? Payments { get; set; }

        /// <summary>
        /// The URL to the customer retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
