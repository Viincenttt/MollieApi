using System.Collections.Generic;

namespace Mollie.Api.Models.List {
    public class ListResponse<T> : ListResponseSimple<T> {
        public int Offset { get; set; }
        public int TotalCount { get; set; }
        public ListResponseLinks Links { get; set; }
    }

    public class ListResponseSimple<T> {
        public int Count { get; set; }

        public List<T> Data { get; set; }
    }
}