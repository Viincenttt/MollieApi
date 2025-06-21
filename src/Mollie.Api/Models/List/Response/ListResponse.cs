using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.List.Response {
    public record ListResponse<T> where T : class {
        public int Count { get; set; }

        [JsonConverter(typeof(ListResponseConverter))]
        [JsonPropertyName("_embedded")]
        public required List<T> Items { get; set; }

        [JsonPropertyName("_links")]
        public required ListResponseLinks<T> Links { get; set; }
    }
}
