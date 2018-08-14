using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List {
    public class ListResponse<T> {
        
        public int Count { get; set; }
        
        [JsonProperty("_embedded")]
        public T Embedded { get; set; }

        [JsonProperty("_links")]
        public ListResponseLinks<T> Links { get; set; }
    }

    public class ListResponseSimple<T> {
        public int Count { get; set; }

        public List<T> Data { get; set; }
    }
}