using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Permission.Response {
    public record PermissionResponseLinks {
        /// <summary>
        /// The API resource URL of the permission itself.
        /// </summary>
        public required UrlObjectLink<PermissionResponse> Self { get; set; }

        /// <summary>
        /// The URL to the permission retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
