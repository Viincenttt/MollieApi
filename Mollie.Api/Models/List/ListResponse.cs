using System.Collections.Generic;

namespace Mollie.Api.Models.List {

    using Newtonsoft.Json;

    using Payment.Response;

    public class ListResponse<T> where T : class {
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public ListResponseLinks Links { get; set; }
        [JsonProperty("_embedded")]
        public T Embedded { get; set; }
        public List<T> Data { get; set; }
    }

    public class EmbeddedPaymentListData {
        public List<PaymentResponse> Payments { get; set; }
    }

    public class ListResponseSimple<T> {
        public int Count { get; set; }
        public List<T> Data { get; set; }
    }
}