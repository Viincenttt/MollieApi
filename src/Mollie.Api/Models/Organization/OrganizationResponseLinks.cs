using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.Invoice.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Organization {
    public record OrganizationResponseLinks {
        /// <summary>
        /// The API resource URL of the organization itself.
        /// </summary>
        public required UrlObjectLink<OrganizationResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s chargebacks can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<ChargebackResponse>>? Chargebacks { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s customers can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<CustomerResponse>>? Customers { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s invoices can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<InvoiceResponse>>? Invoices { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s payments can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<PaymentResponse>>? Payments { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s profiles can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<ProfileResponse>>? Profiles { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s refunds can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<RefundResponse>>? Refunds { get; set; }

        /// <summary>
        /// The API resource URL where the organization’s settlements can be retrieved.
        /// </summary>
        public UrlObjectLink<ListResponse<SettlementResponse>>? Settlements { get; set; }

        /// <summary>
        /// The URL to the organization dashboard
        /// </summary>
        public required UrlLink Dashboard { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
