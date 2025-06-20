using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Payment.Response {
    public record PaymentResponseLinks {
        /// <summary>
        /// The API resource URL of the payment itself.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Self { get; set; }

        /// <summary>
        /// The URL your customer should visit to make the payment. This is where you should redirect the consumer to.
        /// </summary>
        public UrlLink? Checkout { get; set; }

        /// <summary>
        /// The deeplink URL to the app of the payment method. Currently only available for bancontact.
        /// </summary>
        public UrlLink? MobileAppCheckout { get; set; }

        /// <summary>
        /// Direct link to the payment in the Mollie Dashboard.
        /// </summary>
        public required UrlLink Dashboard { get; set; }

        /// <summary>
        /// The API resource URL of the refunds that belong to this payment.
        /// </summary>
        public UrlLink? Refunds { get; set; }

        /// <summary>
        /// The API resource URL of the chargebacks that belong to this payment.
        /// </summary>
        public UrlObjectLink<ListResponse<ChargebackResponse>>? Chargebacks { get; set; }

        /// <summary>
        /// The API resource URL of the captures that belong to this payment.
        /// </summary>
        public UrlLink? Captures { get; set; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObjectLink<SettlementResponse>? Settlement { get; set; }

        /// <summary>
        /// The URL to the payment retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }

        /// <summary>
        /// The API resource URL of the mandate linked to this payment. Not present if a one-off payment.
        /// </summary>
        public UrlObjectLink<MandateResponse>? Mandate { get; set; }

        /// <summary>
        /// The API resource URL of the subscription this payment is part of. Not present if not a subscription payment.
        /// </summary>
        public UrlObjectLink<SubscriptionResponse>? Subscription { get; set; }

        /// <summary>
        /// The API resource URL of the customer this payment belongs to. Not present if not linked to a customer.
        /// </summary>
        public UrlObjectLink<CustomerResponse>? Customer { get; set; }

        /// <summary>
        /// Recurring payments do not have a checkout URL, because these payments are executed without any user interaction.
        /// This link is included for test mode recurring payments, and allows you to set the final payment state for such payments.
        /// </summary>
        public UrlLink? ChangePaymentState { get; set; }
    }
}
