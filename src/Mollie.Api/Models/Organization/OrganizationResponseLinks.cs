using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Invoice;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Organization {
    public class OrganizationResponseLinks {
        /// <summary>
        /// The API resource URL of the organization itself.
        /// </summary>
        public required UrlObjectLink<OrganizationResponse> Self { get; init; }

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
        public required UrlLink Dashboard { get; init; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}