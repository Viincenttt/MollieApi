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
    }
}
