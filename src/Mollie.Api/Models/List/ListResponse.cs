using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List {
    public class ListResponse<T> where T : IResponseObject{
        public int Count { get; set; }

        [JsonConverter(typeof(ListResponseConverter))]
        [JsonProperty("_embedded")]
        public List<T> Items { get; set; }

        [JsonProperty("_links")]
        public ListResponseLinks<T> Links { get; set; }
    }

    public class ListResponseSimple<T> {
        public int Count { get; set; }

        public List<T> Data { get; set; }
    }
}