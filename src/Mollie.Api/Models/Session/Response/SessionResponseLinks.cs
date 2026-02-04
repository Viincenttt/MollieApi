using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Session.Response {
    public record SessionResponseLinks {
        /// <summary>
        ///     The API resource URL of the session itself.
        /// </summary>
        public required UrlObjectLink<SessionResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the session is for.
        /// </summary>
        public required UrlObjectLink<CustomerResponse> Customer { get; set; }

        /// <summary>
        /// The API resource URL of the payments that are created by this session. Not present
        /// if no payments yet created.
        /// </summary>
        public UrlObjectLink<ListResponse<PaymentResponse>>? Payments { get; set; }

        /// <summary>
        /// The API resource URL of the website profile on which this session was created.
        /// </summary>
        public required UrlObjectLink<ProfileResponse> Profile { get; set; }

        /// <summary>
        /// The URL to the session retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
