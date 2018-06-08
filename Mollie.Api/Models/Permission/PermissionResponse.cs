using Newtonsoft.Json;

namespace Mollie.Api.Models.Permission {
    public class PermissionResponse {
        /// <summary>
        /// Indicates the response contains a permission object.
        /// Possible values: permission
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The permission's identifier. See OAuth: Permissions for more details about the available permissions.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A short description of what the permission allows.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether this permission is granted to the app by the organization or not.
        /// </summary>
        public bool Granted { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the permission. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public PermissionResponseLinks Links { get; set; }
    }
}