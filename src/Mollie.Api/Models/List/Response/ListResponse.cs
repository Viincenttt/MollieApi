using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Response {
    public record ListResponse<T> where T : class {
        public int Count { get; set; }

        [JsonConverter(typeof(ListResponseConverter))]
        [JsonProperty("_embedded")]
        public required List<T> Items { get; set; }

        [JsonProperty("_links")]
        public required ListResponseLinks<T> Links { get; set; }
    }
}
