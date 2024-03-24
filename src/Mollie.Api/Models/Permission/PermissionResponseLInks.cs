using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Permission {
    public class PermissionResponseLinks {
        /// <summary>
        /// The API resource URL of the permission itself.
        /// </summary>
        public required UrlObjectLink<PermissionResponse> Self { get; init; }

        /// <summary>
        /// The URL to the permission retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}