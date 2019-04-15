using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class OrderLineCancellationRequest {
        public IEnumerable<OrderLineCancellationRequestLine> Lines { get; set; }
    }

    public class OrderLineCancellationRequestLine {
        public string Id { get; set; }
        public int? Quantity { get; set; }
        public Amount Amount { get; set; }
    }
}
