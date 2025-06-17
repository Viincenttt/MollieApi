using System.Text.Json.Serialization;

namespace Mollie.Api.Models.ClientLink.Response {
    public record ClientLinkResponse {
        public required string Id { get; set; }

        public required string Resource { get; set; }

        [JsonPropertyName("_links")]
        public required ClientLinkResponseLinks Links { get; set; }
    }
}
