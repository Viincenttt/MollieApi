using Newtonsoft.Json;

namespace Mollie.Api.Models.ClientLink.Response {
    public record ClientLinkResponse {
        public required string Id { get; init; }
        
        public required string Resource { get; init; }
        
        [JsonProperty("_links")]
        public required ClientLinkResponseLinks Links { get; init; }
    }
}