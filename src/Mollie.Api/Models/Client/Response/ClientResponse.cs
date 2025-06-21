using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Client.Response {
    public record ClientResponse {
        /// <summary>
        /// Indicates the response contains a client object. Will always contain the string client for this resource type.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this client. Example: org_12345678.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The commission object.
        /// </summary>
        public ClientCommissionResponse? Commission { get; set; }

        /// <summary>
        /// The date and time the client organization was created.
        /// </summary>
        public required DateTime OrganizationCreatedAt { get; set; }

        [JsonPropertyName("_embedded")]
        public ClientEmbeddedResponse? Embedded { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order line. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonPropertyName("_links")]
        public required ClientResponseLinks Links { get; set; }
    }
}
