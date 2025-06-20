using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.ClientLink.Response {
    public record ClientLinkResponseLinks {
        public required UrlLink ClientLink { get; set; }
        public required UrlLink Documentation { get; set; }
    }
}
