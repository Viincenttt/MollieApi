using System.Collections.Generic;

namespace Mollie.Api.Models.List {
    public class ListResponse<T> {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public ListResponseLinks Links { get; set; }
    }
}