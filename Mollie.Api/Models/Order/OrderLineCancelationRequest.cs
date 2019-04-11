using System.Collections.Generic;

namespace Mollie.Api.Models.Order {
    public class OrderLineCancelationRequest {
        public IEnumerable<OrderLineCancelationRequestLine> Lines { get; set; }
    }

    public class OrderLineCancelationRequestLine {
        public string Id { get; set; }
        public int? Quantity { get; set; }
        public Amount Amount { get; set; }
    }
}
