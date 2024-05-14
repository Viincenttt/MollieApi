using Newtonsoft.Json;

namespace Mollie.Api.Models.Permission.Response {
    public record PermissionResponse {
        /// <summary>
        /// Indicates the response contains a permission object.
        /// Possible values: permission
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The permission's identifier. See OAuth: Permissions for more details about the available permissions.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// A short description of what the permission allows.
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// Whether this permission is granted to the app by the organization or not.
        /// </summary>
        public bool Granted { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the permission. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required PermissionResponseLinks Links { get; init; }
    }
}
